var selectionRect = {
    element: null,
    previousElement: null,
    currentY: 0,
    currentX: 0,
    originX: 0,
    originY: 0,
    setElement: function (ele) {
        this.previousElement = this.element;
        this.element = ele;
    },
    getNewAttributes: function () {
        var x = this.currentX < this.originX ? this.currentX : this.originX;
        var y = this.currentY < this.originY ? this.currentY : this.originY;
        var width = Math.abs(this.currentX - this.originX);
        var height = Math.abs(this.currentY - this.originY);
        return {
            x: x,
            y: y,
            width: width,
            height: height
        };
    },
    getCurrentAttributes: function () {
        // use plus sign to convert string into number
        var x = +this.element.attr("x");
        var y = +this.element.attr("y");
        var width = +this.element.attr("width");
        var height = +this.element.attr("height");
        return {
            x1: x,
            y1: y,
            x2: x + width,
            y2: y + height
        };
    },
    getCurrentAttributesAsText: function () {
        var attrs = this.getCurrentAttributes();
        return "x1: " + attrs.x1 + " x2: " + attrs.x2 + " y1: " + attrs.y1 + " y2: " + attrs.y2;
    },
    init: function (newX, newY) {
        var rectElement = svg.append("rect")
            .attr({
                rx: 4,
                ry: 4,
                x: 0,
                y: 0,
                width: 0,
                height: 0
            })
            .classed("selection", true);
        this.setElement(rectElement);
        this.originX = newX;
        this.originY = newY;
        this.update(newX, newY);
    },
    update: function (newX, newY) {
        this.currentX = newX;
        this.currentY = newY;
        this.element.attr(this.getNewAttributes());
    },
    dragUpdate: function (deltaX, deltaY) {
        this.originX += deltaX;
        this.originY += deltaY;
        this.currentX += deltaX;
        this.currentY += deltaY;
        this.element.attr(this.getNewAttributes());
    },
    focus: function () {
        this.element
            .style("stroke", "#DE695B")
            .style("stroke-width", "2.5");
    },
    remove: function () {
        this.element.remove();
        this.element = null;
    },
    removePrevious: function () {
        if (this.previousElement) {
            this.previousElement.remove();
        }
    },
    pointInRect: function (mouseX,mouseY) {
        return (this.originX < mouseX && this.currentX > mouseX) &&
            (this.originY < mouseY && this.currentY > mouseY);
    }
};

//var svg = d3.select("svg");
//var clickTime = d3.select("#clicktime");
//var attributesText = d3.select("#attributestext");

var clickInRect = false;
var startPoint = {x:0 , y:0};
var scalable = true;
function dragStart() {
    console.log("dragStart");
    var p = d3.mouse(this);

    if (selectionRect.pointInRect(p[0], p[1]) ) {
        startPoint.x = p[0];
        startPoint.y = p[1];
        clickInRect = true;
    } else if (scalable === true){
        selectionRect.init(p[0], p[1]);
        selectionRect.removePrevious();
        clickInRect = false;
    }
}

function dragMove() {
    console.log("dragMove");
    var p = d3.mouse(this);
    if (clickInRect === true) {
        //drag orginal rect
        selectionRect.dragUpdate(deltaX = p[0] - startPoint.x,
            deltaY = p[1] - startPoint.y);

        startPoint.x = p[0];
        startPoint.y = p[1];

    } else if (scalable === true) {
        selectionRect.update(p[0], p[1]);
        attributesText.text(selectionRect.getCurrentAttributesAsText());
    }
}

function dragEnd() {
    console.log("dragEnd");
    var finalAttributes = selectionRect.getCurrentAttributes();
    console.dir(finalAttributes);
    if (finalAttributes.x2 - finalAttributes.x1 > 1 && finalAttributes.y2 - finalAttributes.y1 > 1) {
        console.log("range selected");
        // range selected
        d3.event.sourceEvent.preventDefault();
        selectionRect.focus();
    } else {
        console.log("single point");
        // single point selected
        selectionRect.remove();
        // trigger click event manually
    }
        saveSvgChangeToServer();
}


