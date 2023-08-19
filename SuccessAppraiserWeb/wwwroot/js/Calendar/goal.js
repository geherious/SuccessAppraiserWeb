import { jsCalendar } from './jsCalendar.js';

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
            dayGoalInfo.innerText = "Day Goal: " + obj.DayGoal;
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

            let statDayInfo = ["Hard", "Avg", "Easy"];
            statDayInfo.forEach(function (entry) {
                let lvl = document.createElement("div");
                lvl.className = "col-lg text-center";
                lvl.innerText = entry + ":" + "0";
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

            this.buildGoal(0);

        }
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

let calendarManager = {
    calendar: null,

    init: function () {

        this.calendar = jsCalendar.new("#calendar", "now", {
            "monthFormat": "month YYYY",
            "dayFormat": "DD",
            "firstDayOfTheWeek": "2"
        });

        this.baseConfigure();
    },

    baseConfigure: function () {
        if (this.calendar == null) throw new Error("Calendar must be initialized (not null)");

        this.calendar.onDateRender(function (date, element, info) {
            // Make weekends bold and red
            if (!info.isCurrent && (date.getDay() == 0 || date.getDay() == 6)) {
                element.style.color = (info.isCurrentMonth) ? '#c32525' : '#ffb4b4';
            }
            else if (!info.isCurrent && (date.getDay() != 0 || date.getDay() != 6)) {
                element.style.color = (info.isCurrentMonth) ? "black" : '#a6a6a6';
            }

            element.style.background = null;
            element.dataset.name = null;

        });
        this.onDateClick = null;
        this.calendar.refresh();
    },

    configureByGoal: function () {
        if (this.calendar == null) throw new Error("Calendar must be initialized (not null)");
        var index = goalManager.selected.id;
        var goal = goalManager.goals[index];
        this.baseConfigure();
        var startDay = new Date(goal.DateStart);
        var endDay = new Date(startDay);
        endDay.setDate(endDay.getDate() + goal.DayGoal - 1);
        if (Date.now() <= endDay) endDay = new Date();
        this.calendar.onDateRender(function (date, element, info) {
            

            if (date >= startDay && date <= endDay) {
                var found = false;
                for (var item in goal.Dates) {
                    var dateItem = goal.Dates[item];
                    if (dateItem.Date.getTime() == date.getTime()) {
                        element.style.background = calendarManager.getColorByName(dateItem.State.Name);
                        element.dataset.name = dateItem.State.Name;
                        found = true;
                        break;
                    }
                }
                if (!found) {

                    if (info.isCurrent) {
                        element.style.background = "#98D7F2";
                    }
                    else {
                        element.style.background = "#e3e3e3";
                    }
                }
            }
        });

        this.calendar.onDateClick(function (event, date) {
            if (date >= startDay && date <= endDay) {
                var element = event.target.closest('td');
                if (element.dataset.name == "Hard") alert("aoaoa");
            }
        })

        this.calendar.refresh();  
    },

    getColorByName: function (name) {
        switch (name) {
            case "Easy":
                return "#79EF82";
            case "Average":
                return "#F5FF62";
            case "Hard":
                return "#FF6462";
            default:
                return "#e3e3e3";
        }
    }

};
calendarManager.init();
await goalManager.init();


