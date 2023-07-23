// Source: https://github.com/chartjs/Chart.js
$(document).ready($(function () {
	//--------------
	// General data
	//--------------

	var pieOptions = {
		maintainAspectRatio: false,
		scales: {
			yAxes: [{
				ticks: {
					min: 0,
					beginAtZero: false,
					display: false
				},
				gridLines: {
					display: false,
					color: "rgba(255,99,164,0.2)"
				}
			}],
			xAxes: [{
				ticks: {
					min: 0,
					beginAtZero: false,
					display: false
				},
				gridLines: {
					display: false
				}
			}]
		}
	};

	// Color determination of the diagrams
	var background = [
		'rgba(255, 99, 132, 0.2)',
		'rgba(54, 162, 235, 0.2)',
		'rgba(255, 206, 86, 0.2)',
		'rgba(75, 192, 192, 0.2)',
		'rgba(153, 102, 255, 0.2)',
		'rgba(255, 159, 64, 0.2)',
		'rgba(255, 0, 0)',
		'rgba(0, 255, 0)',
		'rgba(0, 0, 255)',
		'rgba(192, 192, 192)',
		'rgba(255, 255, 0)',
		'rgba(255, 0, 255)'
	];

	var border = [
		'rgba(255,99,132,1)',
		'rgba(54, 162, 235, 1)',
		'rgba(255, 206, 86, 1)',
		'rgba(75, 192, 192, 1)',
		'rgba(153, 102, 255, 1)',
		'rgba(255, 159, 64, 1)',
		'rgba(255, 0, 0)',
		'rgba(0, 255, 0)',
		'rgba(0, 0, 255)',
		'rgba(192, 192, 192)',
		'rgba(255, 255, 0)',
		'rgba(255, 0, 255)'
	]

	//--------------
	// Status diagram
	//--------------
	var ctxStatus = document.getElementById("Status").getContext('2d');
	var dataStatus = {
		labels: XLabelsStatus,
		datasets: [{
			label: "Status",
			backgroundColor: background,
			borderColor: border,
			borderWidth: 1,
			data: YValuesStatus
		}]
	};
	var chartStatus = new Chart(ctxStatus, {
		options: pieOptions,
		data: dataStatus,
		type: 'pie'
	});

	//--------------
	// Category diagram
	//--------------
	var ctxCategory = document.getElementById("Category").getContext('2d');
	var dataCategory = {
		labels: XLabelsCategory,
		datasets: [{
			label: "Category",
			backgroundColor: background,
			borderColor: border,
			borderWidth: 1,
			data: YValuesCategory
		}]
	};
	var chartCategory = new Chart(ctxCategory, {
		options: pieOptions,
		data: dataCategory,
		type: 'pie'
	});

	//--------------
	// TicketCreated diagram
	//--------------
	var ctxTicketCreated = document.getElementById("TicketCreated").getContext('2d');
	var dataTicketCreated = {
		labels: XLabelsTicketCreated,
		datasets: [{
			label: "TicketCreated",
			backgroundColor: background,
			borderColor: border,
			borderWidth: 1,
			data: YValuesTicketCreated
		}]
	};
	var chartTicketCreated = new Chart(ctxTicketCreated, {
		options: pieOptions,
		data: dataTicketCreated,
		type: 'pie'
	});
}));