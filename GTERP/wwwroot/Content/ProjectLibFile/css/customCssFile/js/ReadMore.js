$(document).ready(function () {
    //length in characters
    var maxLength = 19;
    var ellipsestext = "...";
    var moretext = "more";
    var lesstext = "less";
    $(".showReadMore").each(function () {
        //get the text of paragraph or div
        var myStr = $(this).text();

        // check if it exceeds the maxLength limit
        if ($.trim(myStr).length > maxLength) {
            //get only limited string firts to show text on page load
            var newStr = myStr.substring(0, maxLength);

            //get remaining string         
            var removedStr = myStr.substr(maxLength, $.trim(myStr).length - maxLength);
            // now append the newStr + "..."+ hidden remaining string
            var Newhtml = newStr + '<span class="moreellipses">' + ellipsestext + '</span><span class="morecontent"><span>' + removedStr + '</span>&nbsp;<a href="javascript:void(0)" class="ReadMore">' + moretext + '</a></span>';

            $(this).html(Newhtml);

        }
    });

    //function to show/hide remaining text on ReadMore button click
    $(".ReadMore").click(function () {

        if ($(this).hasClass("less")) {
            $(this).removeClass("less");
            $(this).html(moretext);
        }
        else {
            $(this).addClass("less");
            $(this).html(lesstext);
        }

        $(this).parent().prev().toggle();
        $(this).prev().toggle();
        return false;
    });
});