import { default as parseColor } from './parse-color';

const elementStyles = element => element.ownerDocument.defaultView.getComputedStyle(element);
const fallbackColor = `#000000`;

const resolveColorFromDOM = (element, cssColor) => {
    // Resolve CSS color functions to rgb() format by applying them to a temporary element and reading back the computed color.
    const tempNode = document.createElement('div');
    if (element.parentElement) {
        element.parentElement.appendChild(tempNode);
    } else {
        element.appendChild(tempNode);
    }

    tempNode.style.color = `rgb(from ${cssColor} clamp(0, r, 255) clamp(0, g, 255) clamp(0, b, 255))`;
    const parsedColor = parseColor(elementStyles(tempNode).color, true);
    tempNode.remove();

    if (!parsedColor) {
        // If parsing fails, return the fallback color.
        return fallbackColor;
    }

    return parsedColor.toCssHex();
};

/**
 * Resolves the color of the specified CSS property of an element to an RGB color string.
 *
 * Supports standard CSS color formats and relative color functions (e.g., oklch()) by leveraging the browser's CSS parsing capabilities.
 *
 * @param {Element} element - The DOM element whose color is to be resolved.
 * @param {string} colorProp - The CSS property name (e.g., 'color', 'backgroundColor') whose color is to be resolved.
 * @returns {string} The resolved hex color string (e.g., '#ff0000' or '#ff0000aa').
 */
export function resolveElementColor(element, colorProp) {
    const cssColor = elementStyles(element).getPropertyValue(colorProp);

    // Attempt to parse the color using parseColor, which supports standard CSS color formats.
    let color = parseColor(cssColor, true);
    if (color) {
        return color.toCssHex();
    }

    // If parsing fails (e.g., for relative colors), fall back to resolving the color from the DOM.
    return resolveColorFromDOM(element, cssColor);
}
