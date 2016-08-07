var currentSort = "";
var sortDirection = "Asc";
var startDate = "";
var endDate = "";

function siteInit() {
    // separate function since it needs to be called every time updateTable is called
    addTableHeadListener();

    $('#calendarglyph').click(function () {
        $('#daterange').click();
    });
    $('#daterange').daterangepicker({
        "autoApply": true,
        "opens": "left",
        "locale": {
            "direction": "ltr",
            "format": "MM/DD/YYYY",
            "separator": " - ",
            "applyLabel": "Apply",
            "cancelLabel": "Cancel",
            "fromLabel": "From",
            "toLabel": "To",
            "customRangeLabel": "Custom",
            "daysOfWeek": [
                "Su",
                "Mo",
                "Tu",
                "We",
                "Th",
                "Fr",
                "Sa"
            ],
            "monthNames": [
                "January",
                "February",
                "March",
                "April",
                "May",
                "June",
                "July",
                "August",
                "September",
                "October",
                "November",
                "December"
            ],
            "firstDay": 0
        },
        "startDate": "07/30/2016",
        "endDate": "08/05/2016",
        "minDate": "01/01/1900",
        "maxDate": "01/01/2100"
    }, function (start, end, label) {
        //console.log("New date range selected: " + start.format('YYYY-MM-DD') + " to " + end.format('YYYY-MM-DD') + " (predefined range: " + label + ")");
        startDate = start.format('YYYY-MM-DD');
        endDate = end.format('YYYY-MM-DD');
        updateTable();
    });

    $('#SearchName').submit(function (event) {
        event.preventDefault();
        updateTable();
    });

    $('#ClearFilters').click(function (event) {
        //console.log("clear");
        $('#NameSearch').val('');
        $('#Sites').val(defaultSite);
        $('#lowScore').val('');
        $('#upScore').val('');
        startDate = "";
        endDate = "";
        currentSort = "";
        updateTable();
    });

    $('.filter').change(function () {
        updateTable();
    });

    $('.changePage').click(function (event) {
        updateTable(pageNum + 1);
        //console.log("next page");
    });
}


// ajax call to update table of records
//      newPage argument indicates 
function updateTable(newPage, nextRecord) {
    //console.log("update");
    $.ajax({
        url: '/Result/UpdateTable',
        type: 'POST',
        datatype: 'json',
        data: {
            sort: currentSort + sortDirection,
            name: $('#NameSearch').val(),
            site: $('#Sites').val(),
            lowScore: $('#lowScore').val(),
            upScore: $('#upScore').val(),
            dateStart: startDate,
            dateEnd: endDate,
            page: newPage,
            record: nextRecord
        },
        success: function (data) {
            $('#dataSection').empty();
            $('#dataSection').html(data);
            //console.log("success");
            addTableHeadListener();
        },
        error: function (jx, status, error) {
            alert(error);
        }
    });
}

// called to add listeners every time the partial page is loaded
function addTableHeadListener() {
    $('.tableHead').click(function (event) {
        if (currentSort === "" || currentSort !== this.id.replace("Head", "")) {
            currentSort = this.id.replace("Head", "");
            sortDirection = "Asc";
        } else if (currentSort === this.id.replace("Head", "") && sortDirection === "Asc") {
            sortDirection = "Desc";
        } else if (currentSort === this.id.replace("Head", "") && sortDirection == "Desc") {
            currentSort = "";
            sortDirection = "Asc";
        }
        //console.log("sort");
        updateTable();
    });
}

// handle moving between records on mobile
function changeRecord(dir) {
    var recordNum = parseInt($('#tableData').find('.active-row').attr('id'));
    if (recordNum < modelCount && dir === "next") {
        $('#tableData').find('.active-row').addClass('inactive-row').removeClass('active-row');
        $('#' + (recordNum + 1)).removeClass('inactive-row').addClass('active-row');
        if (recordNum === 1) {
            $('#PrevRecordSpan').removeClass('hidden');
        }
        if (pageNum == pageCount && (recordNum + 1) == modelCount) {
            $('#NextRecordSpan').addClass('hidden');
        }
        else {
            $('#NextRecordSpan').removeClass('hidden');
        }
    } else if (dir === "next" && pageNum < pageCount) {
        updateTable(pageNum+1);
    } else if (dir === "prev" && recordNum != 1) {
        $('#tableData').find('.active-row').addClass('inactive-row').removeClass('active-row');
        $('#' + (recordNum - 1)).removeClass('inactive-row').addClass('active-row');
        if ((recordNum - 1) == 1 && pageNum == 1) {
            $('#PrevRecordSpan').addClass('hidden');
        }
        else {
            $('#PrevRecordSpan').removeClass('hidden');
        }
    } else if (dir == "prev" && pageNum != 1) {
        updateTable((pageNum-1), pageSize);
    }
}