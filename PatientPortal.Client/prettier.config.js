/**
 * @see https://prettier.io/docs/configuration
 * @type {import("prettier").Config}
 */
const config = {
    printWidth: 80,
    trailingComma: "es5",
    bracketSpacing: true,
    tabWidth: 4,
    semi: true,
    singleQuote: false,
    arrowParens: "always",
    tailwindConfig: "./tailwind.config.js",
    plugins: ["prettier-plugin-tailwindcss"],
};

export default config;
