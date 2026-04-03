const fs = require('fs');
const path = require('path');

const packageJson = JSON.parse(fs.readFileSync(path.join(__dirname, '..', 'package.json'), 'utf8'));
const idbVersion = packageJson.dependencies.idb;

const outputDir = path.join(__dirname, 'generated');
if (!fs.existsSync(outputDir)) {
    fs.mkdirSync(outputDir, { recursive: true });
}

const content = `// AUTO-GENERATED from package.json - DO NOT EDIT
export const IDB_VERSION = "${idbVersion}";
`;

fs.writeFileSync(path.join(outputDir, 'idb-version.ts'), content);
console.log(`Generated idb-version.ts with version ${idbVersion}`);
