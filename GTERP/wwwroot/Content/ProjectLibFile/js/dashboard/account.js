//alert('test');

type = ['primary', 'info', 'success', 'warning', 'danger'];



m = {

    initPickColor: function () {
        $('.pick-class-label').click(function () {
            var new_class = $(this).attr('new-class');
            var old_class = $('#display-buttons').attr('data-class');
            var display_div = $('#display-buttons');
            if (display_div.length) {
                var display_buttons = display_div.find('.btn');
                display_buttons.removeClass(old_class);
                display_buttons.addClass(new_class);
                display_div.attr('data-class', new_class);
            }
        });
    },

    initDocChart: function () {
        chartColor = "#FFFFFF";

        // General configuration for the charts with Line gradientStroke
        gradientChartOptionsConfiguration = {
            maintainAspectRatio: false,
            legend: {
                display: false
            },
            tooltips: {
                bodySpacing: 4,
                mode: "nearest",
                intersect: 0,
                position: "nearest",
                xPadding: 10,
                yPadding: 10,
                caretPadding: 10
            },
            responsive: true,
            scales: {
                yAxes: [{
                    display: 0,
                    gridLines: 0,
                    ticks: {
                        display: false
                    },
                    gridLines: {
                        zeroLineColor: "transparent",
                        drawTicks: false,
                        display: false,
                        drawBorder: false
                    }
                }],
                xAxes: [{
                    display: 0,
                    gridLines: 0,
                    ticks: {
                        display: false
                    },
                    gridLines: {
                        zeroLineColor: "transparent",
                        drawTicks: false,
                        display: false,
                        drawBorder: false
                    }
                }]
            },
            layout: {
                padding: {
                    left: 0,
                    right: 0,
                    top: 15,
                    bottom: 15
                }
            }
        };

        ctx = document.getElementById('lineChartExample').getContext("2d");

        gradientStroke = ctx.createLinearGradient(500, 0, 100, 0);
        gradientStroke.addColorStop(0, '#80b6f4');
        gradientStroke.addColorStop(1, chartColor);

        gradientFill = ctx.createLinearGradient(0, 170, 0, 50);
        gradientFill.addColorStop(0, "rgba(128, 182, 244, 0)");
        gradientFill.addColorStop(1, "rgba(249, 99, 59, 0.40)");

        myChart = new Chart(ctx, {
            type: 'line',
            responsive: true,
            data: {
                labels: ["Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "Jan", "Feb", "Mar", "Apr", "May", "Jun"],
                datasets: [{
                    label: "Active Users",
                    borderColor: "#f96332",
                    pointBorderColor: "#FFF",
                    pointBackgroundColor: "#f96332",
                    pointBorderWidth: 2,
                    pointHoverRadius: 4,
                    pointHoverBorderWidth: 1,
                    pointRadius: 4,
                    fill: true,
                    backgroundColor: gradientFill,
                    borderWidth: 2,
                    data: [542, 480, 430, 550, 530, 453, 380, 434, 568, 610, 700, 630]
                }]
            },
            options: gradientChartOptionsConfiguration
        });
    },

    initDashboardPageCharts: function () {


        //console.log(dashboardData);

        gradientChartOptionsConfigurationWithTooltipBlue = {
            maintainAspectRatio: false,
            legend: {
                display: false
            },

            tooltips: {
                backgroundColor: '#f5f5f5',
                titleFontColor: '#333',
                bodyFontColor: '#666',
                bodySpacing: 4,
                xPadding: 12,
                mode: "nearest",
                intersect: 0,
                position: "nearest"
            },
            responsive: true,
            scales: {
                yAxes: [{
                    barPercentage: 1.6,
                    gridLines: {
                        drawBorder: false,
                        color: 'rgba(29,140,248,0.0)',
                        zeroLineColor: "transparent",
                    },
                    ticks: {
                        suggestedMin: 60,
                        suggestedMax: 125,
                        padding: 20,
                        fontColor: "#2380f7"
                    }
                }],

                xAxes: [{
                    barPercentage: 1.6,
                    gridLines: {
                        drawBorder: false,
                        color: 'rgba(29,140,248,0.1)',
                        zeroLineColor: "transparent",
                    },
                    ticks: {
                        padding: 20,
                        fontColor: "#2380f7"
                    }
                }]
            }
        };

        gradientChartOptionsConfigurationWithTooltipPurple = {
            maintainAspectRatio: false,
            legend: {
                display: false
            },

            tooltips: {
                backgroundColor: '#f5f5f5',
                titleFontColor: '#333',
                bodyFontColor: '#666',
                bodySpacing: 4,
                xPadding: 12,
                mode: "nearest",
                intersect: 0,
                position: "nearest"
            },
            responsive: true,
            scales: {
                yAxes: [{
                    barPercentage: 1.6,
                    gridLines: {
                        drawBorder: false,
                        borderDash: [1, 2],
                        color: 'rgba(255, 255, 255, 0.2',
                        //color: 'rgba(29,140,248,0.0)',
                        zeroLineColor: "transparent",
                    },
                    ticks: {
                        suggestedMin: 60,
                        suggestedMax: 125,
                        padding: 20,
                        fontColor: "rgba(255, 255, 255, 0.7)" /*#ccc*/
                    }
                }],

                xAxes: [{
                    barPercentage: 1.6,
                    gridLines: {
                        drawBorder: false,
                        borderDash: [1, 2],
                        color: 'rgba(255, 255, 255, 0.2',
                        //color: 'rgba(225,78,202,0.1)',
                        zeroLineColor: "transparent",
                    },
                    ticks: {
                        padding: 20,
                        fontColor: "rgba(255, 255, 255, 0.7)" /*#ccc*/
                    }
                }]
            }
        };

        gradientChartOptionsConfigurationWithTooltipOrange = {
            maintainAspectRatio: false,
            legend: {
                display: false
            },

            tooltips: {
                backgroundColor: '#f5f5f5',
                titleFontColor: '#333',
                bodyFontColor: '#666',
                bodySpacing: 4,
                xPadding: 12,
                mode: "nearest",
                intersect: 0,
                position: "nearest"
            },
            responsive: true,
            scales: {
                yAxes: [{
                    barPercentage: 1.6,
                    gridLines: {
                        drawBorder: false,
                        borderDash: [1, 2],
                        color: 'rgba(255, 255, 255, 0.2',
                        //color: 'rgba(29,140,248,0.0)',
                        zeroLineColor: "transparent",
                    },
                    ticks: {
                        suggestedMin: 50,
                        suggestedMax: 110,
                        padding: 20,
                        fontColor: "#ff8a76"
                    }
                }],

                xAxes: [{
                    barPercentage: 1.6,
                    gridLines: {
                        drawBorder: false,
                        borderDash: [1, 2],
                        color: 'rgba(255, 255, 255, 0.2',
                        //color: 'rgba(220,53,69,0.1)',
                        zeroLineColor: "transparent",
                    },
                    ticks: {
                        padding: 20,
                        fontColor: "#ff8a76"
                    }
                }]
            }
        };

        gradientChartOptionsConfigurationWithTooltipcashReceipt = {
            maintainAspectRatio: false,
            legend: {
                display: false
            },

            tooltips: {
                backgroundColor: '#f5f5f5',
                titleFontColor: '#333',
                bodyFontColor: '#666',
                bodySpacing: 4,
                xPadding: 12,
                mode: "nearest",
                intersect: 0,
                position: "nearest"
            },
            responsive: true,
            scales: {
                yAxes: [{
                    barPercentage: 1.6,
                    gridLines: {
                        drawBorder: false,
                        borderDash: [1, 2],
                        color: 'rgba(255, 255, 255, 0.2',
                        zeroLineColor: "transparent",
                    },
                    ticks: {
                        suggestedMin: 50,
                        suggestedMax: 125,
                        padding: 20,
                        fontColor: "rgba(255, 255, 255, 0.7)"
                    }
                }],

                xAxes: [{
                    barPercentage: 1.6,
                    gridLines: {
                        drawBorder: false,
                        //color: 'rgba(0,242,195,0.1)',
                        borderDash: [1, 2],
                        color: 'rgba(255, 255, 255, 0.2',
                        zeroLineColor: "transparent",
                    },
                    ticks: {
                        padding: 20,
                        fontColor: "rgba(255, 255, 255, 0.7)"
                    }
                }]
            }
        };


        gradientBarChartConfiguration = {
            maintainAspectRatio: false,
            legend: {
                display: false
            },

            tooltips: {
                backgroundColor: '#f5f5f5',
                titleFontColor: '#333',
                bodyFontColor: '#666',
                bodySpacing: 4,
                xPadding: 12,
                mode: "nearest",
                intersect: 0,
                position: "nearest"
            },
            responsive: true,
            scales: {
                yAxes: [{

                    gridLines: {
                        drawBorder: false,
                        borderDash: [1, 2],
                        color: 'rgba(255, 255, 255, 0.2',
                        //color: 'rgba(29,140,248,0.1)',
                        zeroLineColor: "transparent",
                    },
                    ticks: {
                        beginAtZero: true,
                        suggestedMin: 60,
                        suggestedMax: 120,
                        padding: 10,
                        fontColor: "rgba(255, 255, 255, 0.7)"
                    }
                }],

                xAxes: [{
                    gridLines: {
                        drawBorder: false,
                        borderDash: [1, 2],
                        color: 'rgba(255, 255, 255, 0.2',
                        //color: 'rgba(29,140,248,0.1)',
                        zeroLineColor: "transparent",
                    },
                    ticks: {
                        padding: 10,
                        fontColor: "rgba(255, 255, 255, 0.7)"
                    }
                }]
            }
        };

        gradientBarChartConfigurationWhite = {
            maintainAspectRatio: false,
            legend: {
                display: false
            },

            tooltips: {
                backgroundColor: '#f5f5f5',
                titleFontColor: '#333',
                bodyFontColor: '#666',
                bodySpacing: 4,
                xPadding: 12,
                mode: "nearest",
                intersect: 0,
                position: "nearest"
            },
            responsive: true,
            scales: {
                yAxes: [{
                    gridLines: {
                        drawBorder: false,
                        borderDash: [1, 2],
                        color: 'rgba(255, 255, 255, 0.2',
                        //color: 'rgba(29,140,248,0.1)',
                        zeroLineColor: "transparent",
                    },
                    ticks: {
                        beginAtZero: true,
                        suggestedMin: 60,
                        suggestedMax: 120,
                        padding: 10,
                        fontColor: "rgba(255, 255, 255, 0.7)"
                    }
                }],

                xAxes: [{
                    gridLines: {
                        drawBorder: false,
                        borderDash: [1, 2],
                        color: 'rgba(255, 255, 255, 0.2',
                        //color: 'rgba(29,140,248,0.1)',
                        zeroLineColor: "transparent",
                        gridLineWidth: 0
                    },
                    ticks: {
                        autoSkip: false,
                        maxRotation: 90,
                        minRotation: 90,
                        padding: 10,
                        fontColor: "rgba(255, 255, 255, 0.7)"
                    }
                }]
         }
        };

        //#region cash Payment

        var ctxCashPayment = document.getElementById("chartLineCashPayment").getContext("2d");

        var gradientStroke = ctxCashPayment.createLinearGradient(0, 230, 0, 50);

        gradientStroke.addColorStop(1, 'rgba(72,72,176,0.2)');
        gradientStroke.addColorStop(0.2, 'rgba(72,72,176,0.0)');
        gradientStroke.addColorStop(0, 'rgba(255, 255, 255, 0.8)'); //purple colors

        var cashPaymentChartLabel = [];
        var cashPaymentChartData = [];

        //console.log(dashboardData.TotalCashPayment.length);



        for (var i = 0; i < dashboardData.TotalCashPayment.length; i++) {
            cashPaymentChartLabel.push(dashboardData.TotalCashPayment[i].MonthName);
            cashPaymentChartData.push(dashboardData.TotalCashPayment[i].TotalDocument);

        }
        var data = {
            //labels: ['JUL', 'AUG', 'SEP', 'OCT', 'NOV', 'DEC'],
            labels: cashPaymentChartLabel,
            datasets: [{
                label: "Data",
                fill: true,
                backgroundColor: gradientStroke,
                borderColor: 'rgba(255, 255, 255, 0.8)',
                borderWidth: 2,
                borderDash: [],
                borderDashOffset: 0.0,
                pointBackgroundColor: '#d048b6',
                pointBorderColor: 'rgba(255,255,255,0)',
                pointHoverBackgroundColor: '#d048b6',
                pointBorderWidth: 20,
                pointHoverRadius: 4,
                pointHoverBorderWidth: 15,
                pointRadius: 4,
                //data: [80, 100, 70, 80, 120, 80],
                data: cashPaymentChartData
            }]
        };

        var myChart = new Chart(ctxCashPayment, {
            type: 'line',
            data: data,
            options: gradientChartOptionsConfigurationWithTooltipPurple
        });

        //#endregion

        //#region cash receipt

        var ctxcashReceipt = document.getElementById("chartLineCashReceipt").getContext("2d");

        var gradientStroke = ctxcashReceipt.createLinearGradient(0, 230, 0, 50);

        gradientStroke.addColorStop(1, 'rgba(66,134,121,0.15)');
        gradientStroke.addColorStop(0.4, 'rgba(66,134,121,0.0)'); //cashReceipt colors
        gradientStroke.addColorStop(0, 'rgba(66,134,121,0)'); //cashReceipt colors

        var cashReceiptChartLabel = [];
        var cashReceiptChartData = [];

        //console.log(dashboardData.TotalCashReceipt.length);


        for (var i = 0; i < dashboardData.TotalCashReceipt.length; i++) {
            cashReceiptChartLabel.push(dashboardData.TotalCashReceipt[i].MonthName);
            cashReceiptChartData.push(dashboardData.TotalCashReceipt[i].TotalDocument);

        }

        var data = {
            //labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
            labels: cashReceiptChartLabel,
            datasets: [{
                label: "dataset",
                fill: true,
                backgroundColor: gradientStroke,
                borderColor: 'rgba(255, 255, 255, 0.7)',
                borderWidth: 2,
                borderDash: [],
                borderDashOffset: 0.0,
                pointBackgroundColor: '#00d6b4',
                pointBorderColor: 'rgba(255,255,255,0)',
                pointHoverBackgroundColor: '#00d6b4',
                pointBorderWidth: 20,
                pointHoverRadius: 4,
                pointHoverBorderWidth: 20,
                pointRadius: 4,
                //data: [50, 480, 430, 550, 530, 453],
                data: cashReceiptChartData
            }]
        };

        var myChart = new Chart(ctxcashReceipt, {
            type: 'line',
            data: data,
            options: gradientChartOptionsConfigurationWithTooltipcashReceipt

        });
        //#endregion

        //#region Bank Payment

        var ctxbankPayment = document.getElementById("chartLineBankPayment").getContext("2d");

        var gradientStroke = ctxbankPayment.createLinearGradient(0, 230, 0, 50);

        gradientStroke.addColorStop(1, 'rgba(72,72,176,0.2)');
        gradientStroke.addColorStop(0.2, 'rgba(72,72,176,0.0)');
        gradientStroke.addColorStop(0, 'rgba(255, 255, 255, 0.8)'); //purple colors

        var bankPaymentChartLabel = [];
        var bankPaymentChartData = [];


        //console.log(dashboardData.TotalBankPayment.length);

        for (var i = 0; i < dashboardData.TotalBankPayment.length; i++) {
            bankPaymentChartLabel.push(dashboardData.TotalBankPayment[i].MonthName);
            bankPaymentChartData.push(dashboardData.TotalBankPayment[i].TotalDocument);

        }
        var data = {
            //labels: ['JUL', 'AUG', 'SEP', 'OCT', 'NOV', 'DEC'],
            labels: bankPaymentChartLabel,
            datasets: [{
                label: "Data",
                fill: true,
                backgroundColor: gradientStroke,
                borderColor: 'rgba(255, 255, 255, 0.8)',
                borderWidth: 2,
                borderDash: [],
                borderDashOffset: 0.0,
                pointBackgroundColor: '#d048b6',
                pointBorderColor: 'rgba(255,255,255,0)',
                pointHoverBackgroundColor: '#d048b6',
                pointBorderWidth: 20,
                pointHoverRadius: 4,
                pointHoverBorderWidth: 15,
                pointRadius: 4,
                //data: [80, 100, 70, 80, 120, 80],
                data: bankPaymentChartData
            }]
        };

        var myChart = new Chart(ctxbankPayment, {
            type: 'line',
            data: data,
            options: gradientChartOptionsConfigurationWithTooltipPurple
        });

        //#endregion

        //#region Bank receipt

        var ctxbankReceipt = document.getElementById("chartLineBankReceipt").getContext("2d");

        var gradientStroke = ctxbankReceipt.createLinearGradient(0, 230, 0, 50);

        gradientStroke.addColorStop(1, 'rgba(66,134,121,0.15)');
        gradientStroke.addColorStop(0.4, 'rgba(66,134,121,0.0)'); //cashReceipt colors
        gradientStroke.addColorStop(0, 'rgba(66,134,121,0)'); //cashReceipt colors

        var bankReceiptChartLabel = [];
        var bankReceiptChartData = [];


        //console.log(dashboardData.TotalBankReceipt.length);


        for (var i = 0; i < dashboardData.TotalBankReceipt.length; i++) {
            bankReceiptChartLabel.push(dashboardData.TotalBankReceipt[i].MonthName);
            bankReceiptChartData.push(dashboardData.TotalBankReceipt[i].TotalDocument);

        }

        var data = {
            //labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
            labels: bankReceiptChartLabel,
            datasets: [{
                label: "dataset",
                fill: true,
                backgroundColor: gradientStroke,
                borderColor: 'rgba(255, 255, 255, 0.7)',
                borderWidth: 2,
                borderDash: [],
                borderDashOffset: 0.0,
                pointBackgroundColor: '#00d6b4',
                pointBorderColor: 'rgba(255,255,255,0)',
                pointHoverBackgroundColor: '#00d6b4',
                pointBorderWidth: 20,
                pointHoverRadius: 4,
                pointHoverBorderWidth: 20,
                pointRadius: 4,
                //data: [50, 480, 430, 550, 530, 453],
                data: bankReceiptChartData
            }]
        };

        var myChart = new Chart(ctxbankReceipt, {
            type: 'line',
            data: data,
            options: gradientChartOptionsConfigurationWithTooltipcashReceipt

        });

        //#endregion

        //#region  Contra

        
        var ctxContra = document.getElementById("chartLineContra").getContext("2d");

        var gradientStroke = ctxContra.createLinearGradient(0, 230, 0, 50);

        gradientStroke.addColorStop(1, 'rgba(66,134,121,0.15)');
        gradientStroke.addColorStop(0.4, 'rgba(66,134,121,0.0)'); //cashReceipt colors
        gradientStroke.addColorStop(0, 'rgba(66,134,121,0)'); //cashReceipt colors

        var contraChartLabel = [];
        var contraChartData = [];


        //console.log(dashboardData.TotalContra.length);


        for (var i = 0; i < dashboardData.TotalContra.length; i++) {
            contraChartLabel.push(dashboardData.TotalContra[i].MonthName);
            contraChartData.push(dashboardData.TotalContra[i].TotalDocument);
        }

        var data = {
            //labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
            labels: contraChartLabel,
            datasets: [{
                label: "dataset",
                fill: true,
                backgroundColor: gradientStroke,
                borderColor: 'rgba(255, 255, 255, 0.7)',
                borderWidth: 2,
                borderDash: [],
                borderDashOffset: 0.0,
                pointBackgroundColor: '#00d6b4',
                pointBorderColor: 'rgba(255,255,255,0)',
                pointHoverBackgroundColor: '#00d6b4',
                pointBorderWidth: 20,
                pointHoverRadius: 4,
                pointHoverBorderWidth: 20,
                pointRadius: 4,
                //data: [50, 480, 430, 550, 530, 453],
                data: contraChartData
            }]
        };

        var myChart = new Chart(ctxContra, {
            type: 'line',
            data: data,
            options: gradientChartOptionsConfigurationWithTooltipcashReceipt

        });


        //#endregion

        //#region Journal

        var ctxJournal = document.getElementById("chartLineJournal").getContext("2d");

        var gradientStroke = ctxJournal.createLinearGradient(0, 230, 0, 50);

        gradientStroke.addColorStop(1, 'rgba(66,134,121,0.15)');
        gradientStroke.addColorStop(0.4, 'rgba(66,134,121,0.0)'); //cashReceipt colors
        gradientStroke.addColorStop(0, 'rgba(66,134,121,0)'); //cashReceipt colors

        var journalChartLabel = [];
        var journalChartData = [];


        //console.log(dashboardData.TotalJournal.length);


        for (var i = 0; i < dashboardData.TotalJournal.length; i++) {
            journalChartLabel.push(dashboardData.TotalJournal[i].MonthName);
            journalChartData.push(dashboardData.TotalJournal[i].TotalDocument);
        }

        var data = {
            //labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
            labels: contraChartLabel,
            datasets: [{
                label: "dataset",
                fill: true,
                backgroundColor: gradientStroke,
                borderColor: 'rgba(255, 255, 255, 0.7)',
                borderWidth: 2,
                borderDash: [],
                borderDashOffset: 0.0,
                pointBackgroundColor: '#00d6b4',
                pointBorderColor: 'rgba(255,255,255,0)',
                pointHoverBackgroundColor: '#00d6b4',
                pointBorderWidth: 20,
                pointHoverRadius: 4,
                pointHoverBorderWidth: 20,
                pointRadius: 4,
                //data: [50, 480, 430, 550, 530, 453],
                data: journalChartData
            }]
        };

        var myChart = new Chart(ctxJournal, {
            type: 'line',
            data: data,
            options: gradientChartOptionsConfigurationWithTooltipcashReceipt

        });


        //#endregion



                //#region Journal


        /////// 3rd lavel

        var chart_labels = ["Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "Jan", "Feb", "Mar", "Apr", "May", "Jun"];
        var chart_data = [100, 70, 90, 70, 85, 60, 75, 60, 90, 80, 110, 100];
        var chart_data1 = [50, 40, 60, 100, 50, 60, 75, 60, 59, 80, 110, 30];

        var cdata = [chart_data, chart_data1];//himu



        //console.log('chart line checking done');



        var allUsersAll = dashboardData.MonthUserWiseAllVoucher.map(function (obj) { return obj.username; });
        var allUsers = allUsersAll.filter(function (v, i) { return allUsersAll.indexOf(v) == i; });
        ////console.log(dashboardData.MonthUserWiseAllVoucher);
        ////console.log(allUsersAll);
        ////console.log(allUsers);


        var allDataSet = [];
        var temp = [];
        for (var i = 0; i < allUsers.length; i++) {

            for (var j = 0; j < dashboardData.MonthUserWiseAllVoucher.length; j++) {
                if (allUsers[i] == dashboardData.MonthUserWiseAllVoucher[j].username) {
                    temp.push(dashboardData.MonthUserWiseAllVoucher[j].TotalDocument);
                }

            }
            allDataSet.push({
                label: allUsers[i],
                fill: true,
                backgroundColor: gradientStroke,
                borderColor: '#d346b1',
                borderWidth: 2,
                borderDash: [],
                borderDashOffset: 0.0,
                pointBackgroundColor: '#d346b1',
                pointBorderColor: 'rgba(255,255,255,0)',
                pointHoverBackgroundColor: '#d346b1',
                pointBorderWidth: 20,
                pointHoverRadius: 4,
                pointHoverBorderWidth: 15,
                pointRadius: 4,
                data: temp
            });
            temp = [];
        }

        //console.log(allDataSet);



        var cashPaymentUsers = dashboardData.MonthUserWiseCashPayment.map(function (obj) { return obj.username; });
        cashPaymentUsers = cashPaymentUsers.filter(function (v, i) { return cashPaymentUsers.indexOf(v) == i; });

        var cashPaymentDataSet = [];
        temp = [];
        for (var i = 0; i < cashPaymentUsers.length; i++) {
            for (var j = 0; j < dashboardData.MonthUserWiseCashPayment.length; j++) {
                if (cashPaymentUsers[i] == dashboardData.MonthUserWiseCashPayment[j].username) {
                    temp.push(dashboardData.MonthUserWiseCashPayment[j].TotalDocument);
                }
            }
            cashPaymentDataSet.push({
                label: cashPaymentUsers[i],
                fill: true,
                backgroundColor: gradientStroke,
                borderColor: '#d346b1',
                borderWidth: 2,
                borderDash: [],
                borderDashOffset: 0.0,
                pointBackgroundColor: '#d346b1',
                pointBorderColor: 'rgba(255,255,255,0)',
                pointHoverBackgroundColor: '#d346b1',
                pointBorderWidth: 20,
                pointHoverRadius: 4,
                pointHoverBorderWidth: 15,
                pointRadius: 4,
                data: temp
            });
            temp = [];
        }

        //console.log(cashPaymentDataSet);

        var piUsers = dashboardData.MonthUserWiseCashReceipt.map(function (obj) { return obj.username; });
        piUsers = piUsers.filter(function (v, i) { return piUsers.indexOf(v) == i; });

        var cashreceiptDataSet = [];
        temp = [];
        for (var i = 0; i < piUsers.length; i++) {
            for (var j = 0; j < dashboardData.MonthUserWiseCashReceipt.length; j++) {
                if (piUsers[i] == dashboardData.MonthUserWiseCashReceipt[j].username) {
                    temp.push(dashboardData.MonthUserWiseCashReceipt[j].TotalDocument);
                }
            }
            cashreceiptDataSet.push({
                label: piUsers[i],
                fill: true,
                backgroundColor: gradientStroke,
                borderColor: '#d346b1',
                borderWidth: 2,
                borderDash: [],
                borderDashOffset: 0.0,
                pointBackgroundColor: '#d346b1',
                pointBorderColor: 'rgba(255,255,255,0)',
                pointHoverBackgroundColor: '#d346b1',
                pointBorderWidth: 20,
                pointHoverRadius: 4,
                pointHoverBorderWidth: 15,
                pointRadius: 4,
                data: temp
            });
            temp = [];
        }

        var bblcUsers = dashboardData.MonthUserWiseBankPayment.map(function (obj) { return obj.username; });
        bblcUsers = bblcUsers.filter(function (v, i) { return bblcUsers.indexOf(v) == i; });

        var bblcDataSet = [];
        temp = [];
        for (var i = 0; i < bblcUsers.length; i++) {
            for (var j = 0; j < dashboardData.MonthUserWiseBankPayment.length; j++) {
                if (bblcUsers[i] == dashboardData.MonthUserWiseBankPayment[j].username) {
                    temp.push(dashboardData.MonthUserWiseBankPayment[j].TotalDocument);
                }
            }
            bblcDataSet.push({
                label: bblcUsers[i],
                fill: true,
                backgroundColor: gradientStroke,
                borderColor: '#d346b1',
                borderWidth: 2,
                borderDash: [],
                borderDashOffset: 0.0,
                pointBackgroundColor: '#d346b1',
                pointBorderColor: 'rgba(255,255,255,0)',
                pointHoverBackgroundColor: '#d346b1',
                pointBorderWidth: 20,
                pointHoverRadius: 4,
                pointHoverBorderWidth: 15,
                pointRadius: 4,
                data: temp
            });
            temp = [];
        }




        var deliveryChallan = dashboardData.MonthUserWiseBankReceipt.map(function (obj) { return obj.username; });
        deliveryChallan = deliveryChallan.filter(function (v, i) { return deliveryChallan.indexOf(v) == i; });

        var dcDataSet = [];
        temp = [];
        for (var i = 0; i < deliveryChallan.length; i++) {
            for (var j = 0; j < dashboardData.MonthUserWiseBankReceipt.length; j++) {
                if (deliveryChallan[i] == dashboardData.MonthUserWiseBankReceipt[j].username) {
                    temp.push(dashboardData.MonthUserWiseBankReceipt[j].TotalDocument);
                }
            }
            dcDataSet.push({
                label: deliveryChallan[i],
                fill: true,
                backgroundColor: gradientStroke,
                borderColor: '#d346b1',
                borderWidth: 2,
                borderDash: [],
                borderDashOffset: 0.0,
                pointBackgroundColor: '#d346b1',
                pointBorderColor: 'rgba(255,255,255,0)',
                pointHoverBackgroundColor: '#d346b1',
                pointBorderWidth: 20,
                pointHoverRadius: 4,
                pointHoverBorderWidth: 15,
                pointRadius: 4,
                data: temp
            });
            temp = [];
        }



        var contraUserMonth = dashboardData.MonthUserWiseContra.map(function (obj) { return obj.username; });
        contraUserMonth = contraUserMonth.filter(function (v, i) { return contraUserMonth.indexOf(v) == i; });

        var contraDataSet = [];
        temp = [];
        for (var i = 0; i < contraUserMonth.length; i++) {
            for (var j = 0; j < dashboardData.MonthUserWiseContra.length; j++) {
                if (contraUserMonth[i] == dashboardData.MonthUserWiseContra[j].username) {
                    temp.push(dashboardData.MonthUserWiseContra[j].TotalDocument);
                }
            }
            contraDataSet.push({
                label: contraUserMonth[i],
                fill: true,
                backgroundColor: gradientStroke,
                borderColor: '#d346b1',
                borderWidth: 2,
                borderDash: [],
                borderDashOffset: 0.0,
                pointBackgroundColor: '#d346b1',
                pointBorderColor: 'rgba(255,255,255,0)',
                pointHoverBackgroundColor: '#d346b1',
                pointBorderWidth: 20,
                pointHoverRadius: 4,
                pointHoverBorderWidth: 15,
                pointRadius: 4,
                data: temp
            });
            temp = [];
        }

        var journalUserMonth = dashboardData.MonthUserWiseJournal.map(function (obj) { return obj.username; });
        journalUserMonth = journalUserMonth.filter(function (v, i) { return journalUserMonth.indexOf(v) == i; });

        var journalDataSet = [];
        temp = [];
        for (var i = 0; i < journalUserMonth.length; i++) {
            for (var j = 0; j < dashboardData.MonthUserWiseJournal.length; j++) {
                if (journalUserMonth[i] == dashboardData.MonthUserWiseJournal[j].username) {
                    temp.push(dashboardData.MonthUserWiseJournal[j].TotalDocument);
                }
            }
            journalDataSet.push({
                label: journalUserMonth[i],
                fill: true,
                backgroundColor: gradientStroke,
                borderColor: '#d346b1',
                borderWidth: 2,
                borderDash: [],
                borderDashOffset: 0.0,
                pointBackgroundColor: '#d346b1',
                pointBorderColor: 'rgba(255,255,255,0)',
                pointHoverBackgroundColor: '#d346b1',
                pointBorderWidth: 20,
                pointHoverRadius: 4,
                pointHoverBorderWidth: 15,
                pointRadius: 4,
                data: temp
            });
            temp = [];
        }

        //console.log('3rd level chart line checking done');


        var ctx = document.getElementById("chartBig1").getContext('2d');
        var gradientStroke = ctx.createLinearGradient(0, 230, 0, 50);
        gradientStroke.addColorStop(1, 'rgba(72,72,176,0.1)');
        gradientStroke.addColorStop(0.4, 'rgba(72,72,176,0.0)');
        gradientStroke.addColorStop(0, 'rgba(119,52,169,0)'); //purple colors

        var config = {
            type: 'line',
            data: {
                labels: chart_labels,
                datasets: allDataSet
            },
            options: gradientChartOptionsConfigurationWithTooltipPurple
        };
        var myChartData = new Chart(ctx, config);


        ////console.log(chartDataSets);

        $("#0fahad").click(function () {
            var data = myChartData.config.data;

            data.datasets = allDataSet;
            data.labels = chart_labels;
            myChartData.update();
        });

        $("#1fahad").click(function () {
            var data = myChartData.config.data;

            data.datasets = cashPaymentDataSet;
            data.labels = chart_labels;
            myChartData.update();
        });

        $("#2fahad").click(function () {
            //var chart_data = [80, 120, 105, 110, 95, 105, 90, 100, 80, 95, 70, 120];
            var data = myChartData.config.data;
            data.datasets = cashreceiptDataSet;
            data.labels = chart_labels;
            myChartData.update();
        });

        $("#3fahad").click(function () {
            //var chart_data = [60, 80, 65, 130, 80, 105, 90, 130, 70, 115, 60, 130];
            var data = myChartData.config.data;
            data.datasets = bblcDataSet;
            data.labels = chart_labels;
            myChartData.update();
        });

        $("#4fahad").click(function () {
            //var chart_data = [60, 80, 65, 130, 80, 105, 90, 130, 70, 115, 60, 130];
            var data = myChartData.config.data;
            data.datasets = dcDataSet;
            data.labels = chart_labels;
            myChartData.update();
        });
        $("#5fahad").click(function () {
            //var chart_data = [60, 80, 65, 130, 80, 105, 90, 130, 70, 115, 60, 130];
            var data = myChartData.config.data;
            data.datasets = contraDataSet;
            data.labels = chart_labels;
            myChartData.update();
        });

        $("#6fahad").click(function () {
            //var chart_data = [60, 80, 65, 130, 80, 105, 90, 130, 70, 115, 60, 130];
            var data = myChartData.config.data;
            data.datasets = journalDataSet;
            data.labels = chart_labels;
            myChartData.update();
        });



        //#region Journal

        /////////////////date wise truck challan qty count
        var ctx = document.getElementById("chartLineVoucherDateWise").getContext("2d");
        //var ctx = chartLineVoucherDateWise.getContext('2d');
        var gradientStroke = ctx.createLinearGradient(0, 250, 0, 90);

        //gradientStroke.addColorStop(1, '#9124a3');
        //gradientStroke.addColorStop(0.4, '#cbb4d4');
        //gradientStroke.addColorStop(0.1, 'rgba(255, 255, 255, 0.8)'); //blue colors
        // Add three color stops
        //gradientStroke.addColorStop(0, 'green');
        //gradientStroke.addColorStop(.5, 'cyan');
        //gradientStroke.addColorStop(1, 'rgba(0,0,255,0.5)');
        //gradientStroke.addColorStop(0, "#80b6f4");
        //gradientStroke.addColorStop(0.2, "#94d973");
        //gradientStroke.addColorStop(0.5, "#fad874");
        //gradientStroke.addColorStop(1, "#f49080");
        gradientStroke.addColorStop(0, '#80b6f4');
        gradientStroke.addColorStop(1, '#f49080');

        // Set the fill style and draw a rectangle
        ctx.fillStyle = gradientStroke;
        ctx.fillRect(20, 20, 200, 100);

        var chartLineVoucherDateWiseLabel = [];
        var chartLineVoucherDateWiseData = [];

        for (var i = 0; i < dashboardData.DayWiseVoucher.length; i++) {
            chartLineVoucherDateWiseLabel.push(dashboardData.DayWiseVoucher[i].DateNameString);
            chartLineVoucherDateWiseData.push(dashboardData.DayWiseVoucher[i].TotalDocument);
            //chartLineVoucherDateWiseData.text = "{value}";
            //left += (barwidth + padding + 10);

        }



        var myChart = new Chart(ctx, {
            type: 'bar',
            zoomType: 'xy',
            responsive: true,
            legend: {
                display: false
            },
            data: {
                //labels: ['USA', 'GER', 'AUS', 'UK', 'RO', 'BR', 'USA', 'GER', 'AUS', 'UK', 'RO', 'BR'],
                labels: chartLineVoucherDateWiseLabel,

                datasets: [{
                    label: "Date : ",
                    fill: true,
                    fillColor: "('transition', 'all 0.6s cubic-bezier(0.86, 0, 0.07, 1)')",
                    backgroundColor: gradientStroke,
                    hoverBackgroundColor: gradientStroke,
                    //borderColor: '#1f8ef1',
                    borderWidth: 0,
                    borderDash: [],
                    borderDashOffset: 0.0,
                    borderColor: "#80b6f4",
                    pointBorderColor: "#80b6f4",
                    pointBackgroundColor: "#80b6f4",
                    pointHoverBackgroundColor: "#80b6f4",
                    pointHoverBorderColor: "#80b6f4",
                    pointBorderWidth: 10,
                    pointHoverRadius: 10,
                    pointHoverBorderWidth: 1,
                    //pointHighlightStroke: "rgba(75, 192, 192, 0.2)",
                    pointRadius: 3,
                    fill: false,
                    //borderWidth: 1,
                    //data: [542, 443, 320, 780, 553, 453, 326, 434, 568, 610, 756, 895],
                    data: chartLineVoucherDateWiseData,
                }]

            },
            //options: {
            //    scales: {
            //        xAxes: [{
            //            gridLines: {
            //                offsetGridLines: true
            //            }
            //        }]
            //    }
            //},
            options: gradientBarChartConfigurationWhite
        });
        /////////////////date wise truck challan qty count


        //#endregion


        ////console.log('final level chart line checking done');



    },


    showNotification: function (from, align) {
        color = Math.floor((Math.random() * 4) + 1);

        $.notify({
            icon: "tim-icons icon-bell-55",
            message: "Welcome to <b>Black Dashboard</b> - a beautiful freebie for every web developer."

        }, {
            type: type[color],
            timer: 8000,
            placement: {
                from: from,
                align: align
            }
        });
    }

};


