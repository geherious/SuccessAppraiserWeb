import { caleandar } from './Calendar.js';

function autoCreate() {
    var el = document.getElementById("auto-calendar");
    caleandar(el, [], {})
}
autoCreate();


let goalManager = {
    json: null,
    apiUrl: "/api/goal/usergoals",

    getJson: async function () {
        if (this.json != null) return null;
        let response = await fetch(this.apiUrl);

        if (response.ok) {
            this.json = await response.json();
        } else {
            alert("Ошибка HTTP: " + response.status);
        }
        console.log(this.json)
    },

    createGoalList: function () {
        let container = document.getElementById("goals")
        for (let i = 0; i < this.json.length; i++) {
            let obj = this.json[i];
            let child = document.createElement("div");
            child.className = "goal container w-100 mb-3";

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

            let dateInfo = document.createElement("div");
            dateInfo.className = "col-3"
            body.appendChild(dateInfo);

            let dayGoalInfo = document.createElement("div");
            dayGoalInfo.className = "row";
            dayGoalInfo.innerText = "Day Goal: " + obj.DayGoal;
            dateInfo.appendChild(dayGoalInfo);

            let dayPassedInfo = document.createElement("div");
            dayPassedInfo.className = "row";
            let dateDiff = new Date(new Date() - new Date(obj.DateStart));
            dayPassedInfo.innerText = "Day Passed: " + dateDiff.getDate();
            dateInfo.appendChild(dayPassedInfo);

            child.appendChild(body);    

            container.appendChild(child);
        }
    },
}
await goalManager.getJson();
goalManager.createGoalList();