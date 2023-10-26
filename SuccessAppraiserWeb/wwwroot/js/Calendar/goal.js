let goalManager = {
    goals: null,
    getGoalUrl: "/api/goal/usergoals",
    deleteGoalUrl: "/api/goal/deletegoal",
    selected: {element: null, id: null},

    init: async function () {
        await goalManager.getJson();
        goalManager.createGoalList();
    },

    getJson: async function () {
        if (this.goals != null) return null;
        let response = await fetch(this.getGoalUrl);

        if (response.ok) {
            this.goals = await response.json();

            for (var item in this.goals) {
                this.goals[item].Dates.forEach(function (element) {
                    element.Date = new Date(element.Date);
                });
            }
        } else {
            alert("Ошибка получения списка: " + response.status);
        }
        console.log(this.goals)
    },

    createGoalList: function () {
        let container = document.getElementById("goals")
        for (let i = 0; i < container.childElementCount; i++) {
            container.removeChild(container.lastChild);
        }
        for (let i = 0; i < this.goals.length; i++) {
            let obj = this.goals[i];
            let child = document.createElement("div");
            child.className = "goal container w-100 mb-3";
            child.dataset.id = obj.Id;
            child.onclick = this.selectGoalOnCLick;
            container.appendChild(child);


            // Header

            let header = document.createElement("div");
            header.className = "row border-bottom justify-content-between"

            let name = document.createElement("div");
            name.className = "col";
            name.innerText = obj.Name;

            let date = document.createElement("div")
            date.className = "col text-end";
            date.innerText = new Date(obj.DateStart).toLocaleDateString();

            header.append(name, date);
            child.appendChild(header);

            //Body

            let body = document.createElement("div");
            body.className = "row";

            //Body.DateInfo

            let dateInfo = document.createElement("div");
            dateInfo.className = "col-3"
            body.appendChild(dateInfo);

            let dayGoalInfo = document.createElement("div");
            dayGoalInfo.className = "row";
            dayGoalInfo.innerText = "Day Goal: " + obj.DaysNumber;
            dateInfo.appendChild(dayGoalInfo);

            let dayPassedInfo = document.createElement("div");
            dayPassedInfo.className = "row";
            let dateDiff = this.getDateDiffDay(new Date(), new Date(obj.DateStart));
            dayPassedInfo.innerText = "Day Passed: " + dateDiff;
            dateInfo.appendChild(dayPassedInfo);

            //Body.DayStat

            let dayStat = document.createElement("div");
            dayStat.className = "col-6"
            body.appendChild(dayStat);

            let dayStatText = document.createElement("div");
            dayStatText.className = "row-12 text-center d-flex align-items-center justify-content-center";
            dayStatText.innerText = "Day States";
            dayStat.appendChild(dayStatText);

            let stats = document.createElement("div");
            stats.className = "row";
            dayStat.appendChild(stats);

            let statDayInfo = goalManager.goals[i].Template.States;
            statDayInfo.forEach(function (entry) {
                let lvl = document.createElement("div");
                lvl.className = "col-lg text-center";
                lvl.innerText = entry.Name + ":" + "0";
                stats.appendChild(lvl);
            })

            //Body.Settings

            let settings = document.createElement("div");
            settings.className = "col-3";
            body.appendChild(settings);

            let settingsName = document.createElement("div");
            settingsName.className = "row text-center align-items-center justify-content-center";
            settingsName.innerText = "Settings";
            settings.appendChild(settingsName);

            let deleteRow = document.createElement("div");
            deleteRow.className = "row";
            settings.appendChild(deleteRow);

            let deleteBtn = document.createElement("button");
            deleteBtn.className = "btn btn-danger delete-btn";
            deleteBtn.textContent = "Delete";
            deleteBtn.addEventListener("click", this.deleteGoal);
            deleteRow.appendChild(deleteBtn);

            //Body.Description
            let desc = document.createElement("div");
            desc.className = "row border-top";
            desc.innerText = obj.Description;
            body.appendChild(desc);


            child.appendChild(body);

        }
        this.buildGoal(0);
    },

    getDateDiffDay: function (date1, date2) {
        let diff = Math.abs(date1.getTime() - date2.getTime());
        let days = Math.ceil(diff / (1000 * 3600 * 24));
        return days;
    },

    deleteGoal: async function (e) {
        const elem = e.target.closest('.goal');
        const btn = e.target.closest('.delete-btn');
        btn.disabled = true;

        let response = await fetch("/api/goal/deletegoal?id=" + elem.dataset.id, {
            method: 'POST',

        });
        if (response.ok) {
            btn.disabled = false;
            elem.parentNode.removeChild(elem);
        }
        else {
            alert("Ошибка удаления");
        }
        if (response.status > 0) {
            goalManager.selected.element = null;
            goalManager.selected.id = null;
            goalManager.buildGoal(0);
        }

    },

    buildGoal: function (index) {
        if (this.goals.length == 0 || index >= this.goals.length) return;

        var container = document.getElementById("goals");
        if (this.selected.element != null) this.selected.element.classList.remove("goal-selected");
        this.selected.element = container.children[index];
        this.selected.element.classList.add("goal-selected");
        this.selected.id = index;
        calendarManager.configureByGoal();


    },

    selectGoalOnCLick: function (e) {
        const child = e.target.closest('.goal');
        var parent = child.parentNode;
        var index = Array.prototype.indexOf.call(parent.children, child);
        goalManager.buildGoal(index);
    },

}

let painting = {
    baseColor: "#bababa",
    paintElement: function (element, color) {
        element.style.backgroundColor = color;
        element.style.color = this.invertColor(color, true);
        let borderColor = this.colorShade(color, -50);
        element.style.border = `thin solid ${borderColor}`;
    },

    invertColor: function (hex, bw) {
        if (hex.indexOf('#') === 0) {
            hex = hex.slice(1);
        }
        // convert 3-digit hex to 6-digits.
        if (hex.length === 3) {
            hex = hex[0] + hex[0] + hex[1] + hex[1] + hex[2] + hex[2];
        }
        if (hex.length !== 6) {
            throw new Error('Invalid HEX color.');
        }
        var r = parseInt(hex.slice(0, 2), 16),
            g = parseInt(hex.slice(2, 4), 16),
            b = parseInt(hex.slice(4, 6), 16);
        if (bw) {
            return (r * 0.299 + g * 0.587 + b * 0.114) > 186
                ? '#000000'
                : '#FFFFFF';
        }
        // invert color components
        r = (255 - r).toString(16);
        g = (255 - g).toString(16);
        b = (255 - b).toString(16);
        // pad each with zeros and return
        return "#" + padZero(r) + padZero(g) + padZero(b);
    },

    colorShade: function (col, amt) {
        col = col.replace(/^#/, '')
        if (col.length === 3) col = col[0] + col[0] + col[1] + col[1] + col[2] + col[2]

        let [r, g, b] = col.match(/.{2}/g);
        ([r, g, b] = [parseInt(r, 16) + amt, parseInt(g, 16) + amt, parseInt(b, 16) + amt])

        r = Math.max(Math.min(255, r), 0).toString(16)
        g = Math.max(Math.min(255, g), 0).toString(16)
        b = Math.max(Math.min(255, b), 0).toString(16)

        const rr = (r.length < 2 ? '0' : '') + r
        const gg = (g.length < 2 ? '0' : '') + g
        const bb = (b.length < 2 ? '0' : '') + b

        return `#${rr}${gg}${bb}`
    }
}

let calendarManager = {
    calendar: null,

    init: function () {

        var options = {
            actions: {
                clickDay(event, dates) {
                },
            },
        };

        this.calendar = new VanillaCalendar('#calendar', options);
        this.calendar.init();

    },

    configureByGoal: function () {
        if (this.calendar == null) throw new Error("Calendar must be initialized (not null)");
        if (goalManager.selected.element == null || goalManager.selected.id == null) return;
        var index = goalManager.selected.id;
        var goal = goalManager.goals[index];
        this.calendar.reset();

        var startDay = new Date(goal.DateStart);  
        var endDay = new Date(startDay);
        endDay.setDate(endDay.getDate() + goal.DaysNumber - 1);
        if (Date.now() <= endDay) endDay = new Date();

        let lastInd = 0;

   
        this.calendar.actions.getDays = function getDays(day, date, HTMLElement, HTMLButtonElement) {
            
            date = new Date(date);
            date.setHours(0, 0, 0, 0);

            if (endDay >= date && date >= startDay) {
                let found = false;
                for (let i = lastInd; i < goal.Dates.length; i++) {
                    if (goal.Dates[i].Date.getTime() == date.getTime()) {
                        let color = '#' + goal.Dates[i].State.Color;
                        painting.paintElement(HTMLButtonElement, color);

                        let state = goal.Dates[i].State.Name;
                        HTMLButtonElement.dataset.state = state;

                        lastInd = i;
                        found = true;
                        break;
                    }
                }
                if (!found) {
                    painting.paintElement(HTMLButtonElement, painting.baseColor);
                }
            }
        }
        this.calendar.update();



        


    }
};
calendarManager.init();
await goalManager.init();


