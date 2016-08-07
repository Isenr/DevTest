function siteMain() {
    var currentSort = "";
    var sortDirection = "Asc";

    createDatePicker();

    $('#SearchName').submit(function (event) {
        event.preventDefault();
        updateTable();
    });

    $('#ClearFilters').click(function (event) {
        console.log("clear");
        $('#NameSearch').val('');
        $('#Sites').val("@ViewBag.DefaultSite");
        $('#lowScore').val('');
        $('#upScore').val('');
        updateTable();
    });

    $('.filter').change(function () {
        updateTable();
    });

    $('.changePage').click(function (event) {
        console.log(this.Attr("data-value"));
        updateTable(@Model.PageNumber+1);
        console.log("next page");
    });

    $('.tableHead').click(function (event) {
        console.log("sort");
        if (currentSort === "" || currentSort !== this.id.replace("Head", "")) {
            currentSort = this.id.replace("Head", "");
            console.log("1");
            sortDirection = "Asc";
        } else if (currentSort === this.id.replace("Head", "") && sortDirection === "Asc") {
            sortDirection = "Desc";
            console.log("2");
        } else if (currentSort === this.id.replace("Head", "") && sortDirection == "Desc") {
            currentSort = "";
            sortDirection = "Asc";
            console.log("3");
        }
        updateTable();
    });

    // date range
    function createDatePicker() {
        // bootstrap-datepicker
        // https://eternicode.github.io/bootstrap-datepicker

        // multidate
        //$('.input-group.date').datepicker({
        //    format: "dd/mm/yyyy",
        //    multidate: true,
        //    multidateSeparator: "-"
        //});

        // date range
        $('.input-group.date').datepicker({
            format: "dd/mm/yyyy"
        });
        $('.input-daterange').datepicker()
            .on("show", function (e) {
                $(".datepicker-days").css('display', 'block');
                console.log("show");
            });
    }
}