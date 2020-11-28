// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
let ctxL = document.getElementById("lineChart").getContext('2d');
let adudata = [0, 3, 15, 13, 15, 25, 3, 0, 12, 10, 55, 2];
let myLineChart = new Chart(ctxL, {
    type: 'line',
    data: {
        labels: ["January", "February", "March", "April", "May", "June", "July","August","September","October","November","December"],
        datasets: [{
            label: "Average Daily Users",
            data: adudata,
            borderColor: [
                'rgba(200, 99, 132, .7)',
            ],
            borderWidth: 3
        },
        {
            label: "Registered Users",
            data: [3,6,9,9,12,13,15,18,33,34,40,41],
            borderColor: [
                'rgba(0, 10, 130, .7)',
            ],
            borderWidth: 3
        }
        ]
    },
    options: {
        responsive: true
    }
});