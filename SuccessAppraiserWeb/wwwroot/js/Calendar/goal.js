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
            settings.innerText = "Setting";
            body.appendChild(settings);

            //Body.Description
            let desc = document.createElement("div");
            desc.className = "row border-top";
            desc.innerText = obj.Description;
            body.appendChild(desc);


            child.appendChild(body);    

            container.appendChild(child);
        }
    },

    getDateDiffDay: function (date1, date2) {
        let diff = Math.abs(date1.getTime() - date2.getTime());
        let days = Math.ceil(diff / (1000 * 3600 * 24));
        return days;
    }
}
await goalManager.getJson();
goalManager.createGoalList();