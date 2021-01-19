let ctxL = document.getElementById("lineChart").getContext('2d');
let ctxt = document.getElementById("RecipesnArticlesChar").getContext('2d');

$(document).ready(function () {
    $.ajax({
        url: '/api/statistics',
        type: 'GET',
        dataType: "json",
        success: function (data) {
            myLineChart.data.datasets[1].data = data.registeredUsers;
            myDoughnutChart.data.datasets[0].data = [data.articlesCount, data.recipesCount, 0, 0];
            myDoughnutChart.data.datasets[1].data = [0, 0, data.commentsCount, data.reviewsCount];

            myLineChart.update();
            myDoughnutChart.update();

        }
    })
});

let myLineChart = new Chart(ctxL, {
    type: 'line',
    data: {
        labels: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"],
        datasets: [{
            label: "Average Daily Users",
            data: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
            borderColor: [
                'rgba(200, 99, 132, .7)',
            ],
            borderWidth: 3
        },
        {
            label: "Registered Users",
            data: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
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

let myDoughnutChart = new Chart(ctxt, {
    type: 'pie',
    data: {
        labels: ["Articles", "Recipes", "Article Comments", "Reviews"],
        datasets: [{

            data: [0, 0, 0, 0],
            backgroundColor: ["#3e95cd", "#c45850", "#52BE80", "#F7DC6F"],
        }, {
            data: [0, 0, 0, 0],
            backgroundColor: ["#3e95cd", "#c45850", "#52BE80", "#F7DC6F"],
        }]

    },
    options: {
        responsive: false,
        legend: {
            position: "right",
            align: "middle"
        },
    }
});
$(document).ready(function () {
    $.ajax({
        type: 'GET',
        data: { type: "Categories" },
        url: '/Administration/Dashboard/FetchTopFive',
        success: function (data) {
            $('#topCategories').append(`<div class="br-b-white p-3">Top Categories</div>`);
            data.forEach(el => $('#topCategories').append(`<div class="pt-3 sm">` + el.key + " - " + el.count + ` Recipes</div>`));
        }
    });
    $.ajax({
        type: 'GET',
        data: { type: "Recipes" },
        url: '/Administration/Dashboard/FetchTopFive',
        success: function (data) {
            $('#topRecipes').append(`<div class="br-b-white p-3">Top Recipes</div>`);
            data.forEach(el => $('#topRecipes').append(`<div class="pt-3 sm">` + el.key + " - " + el.rate + ` <i class="fas fa-star star"></i></div>`));
        }
    });
    $.ajax({
        type: 'GET',
        data: { type: "Articles" },
        url: '/Administration/Dashboard/FetchTopFive',
        success: function (data) {
            $('#topComentedArticle').append(`<div class="br-b-white p-3">Top Commented Ariticles</div>`);
            data.forEach(el => $('#topComentedArticle').append(`<div class="pt-3 sm">` + el.key + " - " + el.count + ` Comments</div>`));
        }
    });
});