function measureHeight(element) {
    return Array.prototype.reduce.call(element.childNodes, function (totalHeight, childNode) {
        return totalHeight + (childNode.offsetHeight || 0);
    }, 0);
}

function toggleExpand(element) {
    if (!element.style.height || element.style.height === '0px') {
        element.style.height = measureHeight(element) + 'px';
    } else {
        element.style.height = '0px';
    }
}