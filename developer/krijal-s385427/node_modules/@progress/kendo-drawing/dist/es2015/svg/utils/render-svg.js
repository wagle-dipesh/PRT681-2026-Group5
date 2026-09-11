import { prependElement, replaceStyleAttr, restoreStyleAttr } from '../../util/element-set-styles-safe';
import { SVG_NS } from '../constants';

const renderUsingInnerHTML = (container, svg) => {
    prependElement(container, svg);
};

const renderUsingDOMParser = (container, svg) => {
    const parser = new DOMParser();
    const chartDoc = parser.parseFromString(replaceStyleAttr(svg), "text/xml");
    restoreStyleAttr(chartDoc);
    const importedDoc = document.adoptNode(chartDoc.documentElement);

    container.insertBefore(importedDoc, container.firstChild);
};

let implementation;

const renderSVG = (container, svg) => {
    if (implementation) {
        return implementation(container, svg);
    }

    implementation = renderUsingInnerHTML;

    if (typeof document !== "undefined") {
        const testFragment = "<svg xmlns='" + SVG_NS + "'></svg>";
        const testContainer = document.createElement("div");
        const hasParser = typeof DOMParser !== "undefined";

        testContainer.innerHTML = testFragment;

        if (hasParser && testContainer.firstChild.namespaceURI !== SVG_NS) {
            implementation = renderUsingDOMParser;
        }
    }

    return implementation(container, svg);
};

export default renderSVG;
