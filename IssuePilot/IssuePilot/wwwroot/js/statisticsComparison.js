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
	// Status diagram of the first project
	//--------------
	var ctxStatusFirst = document.getElementById("StatusFirst").getContext('2d');
	var dataStatusFirst = {
		labels: XLabelsStatusFirst,
		datasets: [{
			label: "Status",
			backgroundColor: background,
			borderColor: border,
			borderWidth: 1,
			data: YValuesStatusFirst
		}]
	};
	var chartStatusFirst = new Chart(ctxStatusFirst, {
		options: pieOptions,
		data: dataStatusFirst,
		type: 'pie'
	});

	//--------------
	// Category diagram of the first project
	//--------------
	var ctxCategoryFirst = document.getElementById("CategoryFirst").getContext('2d');
	var dataCategoryFirst = {
		labels: XLabelsCategoryFirst,
		datasets: [{
			label: "Category",
			backgroundColor: background,
			borderColor: border,
			borderWidth: 1,
			data: YValuesCategoryFirst
		}]
	};
	var chartCategoryFirst = new Chart(ctxCategoryFirst, {
		options: pieOptions,
		data: dataCategoryFirst,
		type: 'pie'
	});

	//--------------
	// Ticket created diagram of the first project
	//--------------
	var ctxTicketCreatedFirst = document.getElementById("TicketCreatedFirst").getContext('2d');
	var dataTicketCreatedFirst = {
		labels: XLabelsTicketCreatedFirst,
		datasets: [{
			label: "TicketCreated",
			backgroundColor: background,
			borderColor: border,
			borderWidth: 1,
			data: YValuesTicketCreatedFirst
		}]
	};
	var chartTicketCreatedFirst = new Chart(ctxTicketCreatedFirst, {
		options: pieOptions,
		data: dataTicketCreatedFirst,
		type: 'pie'
	});

	//--------------
	// Status diagram of the second project
	//--------------
	var ctxStatusSecond = document.getElementById("StatusSecond").getContext('2d');
	var dataStatusSecond = {
		labels: XLabelsStatusSecond,
		datasets: [{
			label: "Status",
			backgroundColor: background,
			borderColor: border,
			borderWidth: 1,
			data: YValuesStatusSecond
		}]
	};
	var chartStatusSecond = new Chart(ctxStatusSecond, {
		options: pieOptions,
		data: dataStatusSecond,
		type: 'pie'
	});

	//--------------
	// Category diagram of the second project
	//--------------
	var ctxCategorySecond = document.getElementById("CategorySecond").getContext('2d');
	var dataCategorySecond = {
		labels: XLabelsCategorySecond,
		datasets: [{
			label: "Category",
			backgroundColor: background,
			borderColor: border,
			borderWidth: 1,
			data: YValuesCategorySecond
		}]
	};
	var chartCategorySecond = new Chart(ctxCategorySecond, {
		options: pieOptions,
		data: dataCategorySecond,
		type: 'pie'
	});

	//--------------
	// Ticket created diagram of the second project
	//--------------
	var ctxTicketCreatedSecond = document.getElementById("TicketCreatedSecond").getContext('2d');
	var dataTicketCreatedSecond = {
		labels: XLabelsTicketCreatedSecond,
		datasets: [{
			label: "TicketCreated",
			backgroundColor: background,
			borderColor: border,
			borderWidth: 1,
			data: YValuesTicketCreatedSecond
		}]
	};
	var chartTicketCreatedSecond = new Chart(ctxTicketCreatedSecond, {
		options: pieOptions,
		data: dataTicketCreatedSecond,
		type: 'pie'
	});

}));