//Cannot be added to the _WritingPreview partial view because @section doesn't work!
//File must be sourced on the layout page or in a section on the page that is calling _WritingPreview

(function () {
    var scrollHandle = 0,
        scrollStep = 5,
        parent,
        wId;

    //Start the scrolling process
    $(".scrollArrow").on("mousedown", function () {
        var data = $(this).data('scrollModifier'),
            direction = parseInt(data, 10),
            wData = $(this).data('parent');

        wId = parseInt(wData);

        parent = $(".tagScroller." + wId + " .tagScrollbar");
        $(this).addClass('active');

        startScrolling(direction, scrollStep);
    });

    //Kill the scrolling
    $(".scrollArrow").on("mouseup", function () {
        stopScrolling();
        $(this).removeClass('active');
    });

    //Actual handling of the scrolling
    function startScrolling(modifier, step) {
        if (scrollHandle === 0) {
            scrollHandle = setInterval(function () {
                var newOffset = parent.scrollLeft() + (scrollStep * modifier);
                parent.scrollLeft(newOffset);
            }, 10);
        }
    }

    function stopScrolling() {
        clearInterval(scrollHandle);
        scrollHandle = 0;
    }
})();