import { caleandar } from './Calendar.js';

function autoCreate() {
    var el = document.getElementById("auto-calendar");
    caleandar(el, [], {})
}
autoCreate();


let goalManager = {
    goals: null,
    getGoalUrl: "/api/goal/usergoals",
    deleteGoalUrl: "/api/goal/deletegoal",
    selected: null,

    getJson: async function () {
        if (this.goals != null) return null;
        let response = await fetch(this.getGoalUrl);

        if (response.ok) {
            this.goals = await response.json();
            //if (this.goals.length != 0) {
            //    continue;
            //}
        } else {
            alert("Ошибка получения списка: " + response.status);
        }
        console.log(this.goals)
    },

    createGoalList: function () {
        let container = document.getElementById("goals")
        for (let i = 1; i < container.childElementCount; i++) {
            container.removeChild(container.lastChild);
        }
        for (let i = 0; i < this.goals.length; i++) {
            let obj = this.goals[i];
            let child = document.createElement("div");
            child.className = "goal container w-100 mb-3";
            child.id = obj.Id;
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
        
        let response = await fetch("/api/goal/deletegoal?id=" + elem.id, {
            method: 'POST',

        });
        if (response.ok) {
            btn.disabled = false;
            elem.parentNode.removeChild(elem);
        }
        else {
            alert("Ошибка удаления");
        }
        goalManager.selected = null;
        goalManager.buildGoal(0);
    },

    buildGoal: function (index) {
        if (index - 1 >= this.goals.length) return;
        if (index == 0) {
            this.buildGoal(1);
            return;
        }

        var container = document.getElementById("goals");
        if (this.selected != null) this.selected.classList.remove("goal-selected");
        this.selected = container.children[index];
        this.selected.classList.add("goal-selected");

    },

    selectGoalOnCLick: function (e) {
        const child = e.target.closest('.goal');
        var parent = child.parentNode;
        var index = Array.prototype.indexOf.call(parent.children, child);

        goalManager.buildGoal(index);
    },

}
await goalManager.getJson();
goalManager.createGoalList();