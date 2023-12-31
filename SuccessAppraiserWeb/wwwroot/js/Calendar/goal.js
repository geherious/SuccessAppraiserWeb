﻿let goalManager = {
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
        const response = await fetch(this.getGoalUrl);

        if (response.ok) {
            this.goals = await response.json();

            for (let item in this.goals) {
                this.goals[item].Dates.forEach(function (element) {
                    element.Date = new Date(element.Date);
                });
            }
        } else {
            alert(`Ошибка получения списка: ${response.status}`);
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
            dayGoalInfo.innerText = `Day Goal: ${obj.DaysNumber}`;
            dateInfo.appendChild(dayGoalInfo);

            let dayPassedInfo = document.createElement("div");
            dayPassedInfo.className = "row";
            let dateDiff = this.getDateDiffDay(new Date(), new Date(obj.DateStart));
            dayPassedInfo.innerText = `Day Passed: ${dateDiff}`;
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
                const lvl = document.createElement("div");
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
        const diff = Math.abs(date1.getTime() - date2.getTime());
        const days = Math.ceil(diff / (1000 * 3600 * 24));
        return days;
    },

    deleteGoal: async function (e) {
        const elem = e.target.closest('.goal');
        const btn = e.target.closest('.delete-btn');
        btn.disabled = true;

        const response = await fetch(`/api/goal/deletegoal?id=${elem.dataset.id}`, {
            method: 'POST',

        });
        if (response.ok) {
            btn.disabled = false;   
            elem.parentElement.removeChild(elem);
            goalManager.goals.splice(goalManager.selected.id, 1);
            goalManager.selected.element = null;
            goalManager.selected.id = null;
            goalManager.buildGoal(0);
        }
        else {
            // TODO: Toast
            const toastEl = new bootstrap.Toast(document.getElementById("DeleteGoalErrorToast"));
            toastEl.show();
        }
        calendarManager.configureByGoal();

    },

    buildGoal: function (index) {
        if (this.goals.length == 0 || index >= this.goals.length) return;

        const container = document.getElementById("goals");
        if (this.selected.element != null) this.selected.element.classList.remove("goal-selected");
        this.selected.element = container.children[index];
        this.selected.element.classList.add("goal-selected");
        this.selected.id = index;
        calendarManager.configureByGoal();


    },

    selectGoalOnCLick: function (e) {
        const child = e.target.closest(".goal");
        const parent = child.parentNode;
        const index = Array.prototype.indexOf.call(parent.children, child);
        goalManager.buildGoal(index);
    }


}

let painting = {
    baseColor: "#bababa",
    paintElement: function (element, color) {
        element.style.backgroundColor = color;
        element.style.color = this.invertColor(color, true);
        const borderColor = this.colorShade(color, -50);
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

    initCalendar: function () {
        const options = {
            actions: {
                clickDay(event, dates) {
                    if (event.target.dataset.state == "stateless") {
                        calendarManager.openStatelessModal(event, dates);
                    }
                },
            },
        };



        this.calendar = new VanillaCalendar('#calendar', options);
        this.calendar.init();
    },

    AddStatelessModalResetListener: function () {
        let modalEl = document.getElementById("CreateStatelessDayModal");
        modalEl.addEventListener("hidden.bs.modal", function() {
            modalEl.querySelector("form").reset();
        });
    },

    init: function () {
        calendarManager.AddStatelessModalResetListener();
        calendarManager.initCalendar();
    },

    configureByGoal: function () {
        if (this.calendar == null) throw new Error("Calendar must be initialized (not null)");
        if (goalManager.selected.element == null || goalManager.selected.id == null) {
            calendarManager.initCalendar();
            return;
        }
        const index = goalManager.selected.id;
        let goal = goalManager.goals[index];

        var startDay = new Date(goal.DateStart);  
        var endDay = new Date(startDay);
        endDay.setDate(endDay.getDate() + goal.DaysNumber - 1);
        if (Date.now() <= endDay) endDay = new Date();

        this.calendar.actions.getDays = function (day, date, HTMLElement, HTMLButtonElement) {
            HTMLButtonElement.dataset.state = null;
            date = new Date(date);
            date.setHours(0, 0, 0, 0);


            if (endDay >= date && date >= startDay) {
                HTMLButtonElement.dataset.state = "stateless";
                let found = false;
                let lastInd = 0;
                for (let i = lastInd; i < goal.Dates.length; i++) {
                    if (goal.Dates[i].Date.getTime() == date.getTime()) {
                        const color = `#${goal.Dates[i].State.Color}`;
                        painting.paintElement(HTMLButtonElement, color);

                        const state = goal.Dates[i].State.Name;
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
        this.calendar.reset();



        


    },
    openStatelessModal: function (event, dates) {
        const modalElement = document.getElementById('CreateStatelessDayModal');
        const modal = bootstrap.Modal.getOrCreateInstance(modalElement);
        document.getElementById("StatelessDateInput").value = event.target.closest("button").dataset.calendarDay;

        // Add select options
        const selectEl = document.getElementById('StatelessStateSelect');
        const states = goalManager.goals[goalManager.selected.id].Template.States;
        for (let i = 0; i < states.length; i++) {
            const opt = document.createElement('option');
            opt.value = states[i].Id;
            opt.text = states[i].Name;
            selectEl.appendChild(opt);
        }

        // Add goal id
        document.getElementById("StatelessGoalIdInput").value = goalManager.goals[goalManager.selected.id].Id;

        calendarManager.handleStatelessModalSubmit(modalElement, modal);

        modal.show();
    },
    handleStatelessModalSubmit: function (modalElement, modal) {
        const form = modalElement.querySelector("form");

        form.addEventListener("submit", async function (event) {


            event.preventDefault();

            const data = {};
            for (const pair of new FormData(form)) {
                data[pair[0]] = pair[1];
            }

            const response = await fetch("/api/goal/CreateGoalDate",
                {
                    method: 'POST',
                    body: JSON.stringify(data),
                    headers: {
                        'Content-Type': 'application/json'
                    },
                });

            if (!response.ok) {
                modal.hide();
                bootstrap.Toast.getOrCreateInstance(document.getElementById("CreateGoalDateErrorToast")).show();
            } else {
                let goal = goalManager.goals.find(g => g.Id == data["GoalId"]);
                const responseData = await response.json();
                let newState = goal.Template.States.find(s => s.Id == data["StateId"]);
                let newDate = new Date(data["Date"]);
                newDate.setHours(0, 0, 0, 0);
                let newGoalDate = {
                    Id: responseData.id,
                    Date: newDate,
                    Comment: data["Comment"],
                    Goal: null,
                    GoalId: Number(data["GoalId"]),
                    StateId: Number(data["StateId"]),
                    State: newState
                };

                goal.Dates.splice(binarySearch(goal.Dates, newGoalDate, compareGoalDates), 0, newGoalDate);
                calendarManager.configureByGoal();
                modal.hide();

            }
        });
    }
};

function binarySearch(ar, el, compareFn) {
    if (ar.length == 0) return 0;
    if (el.Date < ar[0].Date)
        return 0;
    if (el.Date > ar[ar.length - 1].Date)
        return ar.length;
    var m = 0;
    var n = ar.length - 1;
    while (m <= n) {
        var k = (n + m) >> 1;
        var cmp = compareFn(el, ar[k]);
        if (cmp > 0) {
            m = k + 1;
        } else if (cmp < 0) {
            n = k - 1;
        } else {
            return k;
        }
    }
    return -m - 1;
}

function compareGoalDates(date1, date2) {
    return date1.Date > date2.Date;
}
calendarManager.init();
await goalManager.init();