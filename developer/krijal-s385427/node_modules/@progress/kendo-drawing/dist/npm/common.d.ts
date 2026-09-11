/**
 * @hidden
 * @param value
 * @param safe
 */
export function parseColor(value: string, safe?: boolean): any;

/**
 * Resolves the color of the specified CSS property of an element to an RGB color string.
 *
 * Supports standard CSS color formats and relative color functions (e.g., oklch()) by leveraging the browser's CSS parsing capabilities.
 *
 * @param {Element} element - The DOM element whose color is to be resolved.
 * @param {string} colorProp - The CSS property name (e.g., 'color', 'backgroundColor') whose color is to be resolved.
 * @returns {string} The resolved hex color string (e.g., '#ff0000' or '#ff0000aa').
 */
export function resolveElementColor(element: Element, colorProp: string): string;

/**
 * @hidden
 *
 */
export const namedColors: any;

/**
 * @hidden
 *
 */
export class Color {
    constructor(value: any);

    toHex(): string;
    resolveColor(value: any): any;
    normalizeByte(value: any): any;
    padDigit(value: any): any;
    brightness(value: any): any;

    percBrightness(): number
    static fromBytes(r: any, g: any, b: any, a: any): any;
    static fromRGB(r: any, g: any, b: any, a: any): any;
    static fromHSV(h: any, s: any, v: any, a: any): any;
    static fromHSL(h: any, s: any, l: any, a: any): any;
}
