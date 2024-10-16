function SaveThumbnail() {

    var cloneDom = $("#clone-me").clone(true);
    cloneDom[0].id = "clone";
    $(cloneDom[0]).removeClass("game-canvas");
    cloneDom[0].children[0].id = "clone";
    
    cloneDom.addClass("cloneDom");
    cloneDom.css({
        "background-color": "#fafafa",
        "position": "absolute",
        "z-index": "-1",
        "height": cloneDom.width,
        "width": cloneDom.width,
        "visibility": "hidden"
    });

    if (typeof html2canvas !== 'undefined') {
        var nodesToRecover = [];
        var nodesToRemove = [];
        var svgElem = cloneDom. find ('svg');
        svgElem.each(function (index, node) {
            var parentNode = node.parentNode;
            var svg = node.outerHTML.trim();

            var canvas = document.createElement('canvas');
            canvg(canvas, svg);
            if (node.style.position) {
                canvas.style.position += node.style.position;
                canvas.style.left += node.style.left;
                canvas.style.top += node.style.top;
            }

            nodesToRecover.push({
                parent: parentNode,
                child: node
            });
            parentNode.removeChild(node);

            nodesToRemove.push({
                parent: parentNode,
                child: canvas
            });

            parentNode.appendChild(canvas);
        });
        
        $("body").append(cloneDom);
        
        html2canvas($(".cloneDom")[0], {
            backgroundColor: null,
            width: cloneDom.width,
            heigh: cloneDom.width,
            windowWidth: 1000,
            windowHeight: 1000}).then(function(canvas) {
            var url = canvas.toDataURL("image/png");
            $.post("/save", {
                    value: btoa($(".cloneDom")[0].innerHTML),
                    token: gameToken
                }, {
                    dataType: "json",
                    traditional: true
                }
            );
            cloneDom.remove ();
            $(".cloneDom").remove();
        });
    }
}