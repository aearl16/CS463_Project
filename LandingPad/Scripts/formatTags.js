function showExplanation(Explanation) {
    $("#formatTagDescription").empty();
    $("#formatTagDescription").append(Explanation);
}

//sd stands for singular dependency and md stands for multiple dependency   
function ftChildren(id, sdChildren, mdChildren, dependencies) {
    if ($("span." + id + " input[type=checkbox]").is(':checked'))
        loadChildren(sdChildren, mdChildren, dependencies);
    else {
        for (var i = 0; i < mdChildren.length; i++)
            sdChildren.push(mdChildren[i]);

        unloadChildren(sdChildren);
    }
}

//sd stands for singular dependency and md stands for multiple dependency 
function loadChildren(sdChildren, mdChildren, dependencies) {
    for (var i = 0; i < sdChildren.length; i++) {

        if ($("span." + sdChildren[i]).hasClass("collapse"))
            $("span." + sdChildren[i]).removeClass("collapse");
    }

    for (i = 0; i < mdChildren.length; i++) {
        if ($("span." + dependencies[i] + " input[type=checkbox]").is(':checked')) {
            if ($("span." + dChildren[i]).hasClass("collapse"))
                $("span." + mdChildren[i]).removeClass("collapse");
        }
    }
}

function unloadChildren(children) {
    for (var i = 0; i < children.length; i++) {
        if ($("span." + children[i] + " input[type=checkbox]").is(':checked') !== true) {
            if ($("span." + children[i]).hasClass("collapse") !== true)
                $("span." + children[i]).addClass("collapse");
        }
    }
}