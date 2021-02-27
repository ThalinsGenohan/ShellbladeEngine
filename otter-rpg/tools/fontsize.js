const fs = require('fs');
const { createCanvas, loadImage } = require('canvas');

const sheetSize = {x: 16, y: 6};
const charSize = {x: 8, y: 8};
const totalSize = {x: sheetSize.x * charSize.x, y: sheetSize.y * charSize.y};

const canvas = createCanvas(totalSize.x, totalSize.y);
const ctx = canvas.getContext('2d');

function index(x, y) {
    return (x + (y * totalSize.x)) * 4; // 4 bytes per pixel
}

(async function () {
    const path = process.argv[2];
    const files = await fs.promises.readdir(path);

    for (const file of files) {
        if (!file.match(/.*\.png$/)) continue;

        let fontImage = await loadImage(path + "/" + file);
        ctx.clearRect(0, 0, totalSize.x, totalSize.y);
        ctx.drawImage(fontImage, 0, 0, totalSize.x, totalSize.y);
        let imageData = ctx.getImageData(0, 0, totalSize.x, totalSize.y);
        let data = imageData.data;

        let widths = [];
        for (let y = 0; y < sheetSize.y; y++) {
            widths[y] = [];
            char: for (let x = 0; x < sheetSize.x; x++) {
                col: for (let u = 0; u < charSize.x; u++) {
                    for (let v = 0; v < charSize.y; v++) {
                        let i = index(
                            x * charSize.x + u,
                            y * charSize.y + v,
                        );
                        if (data[i + 3] > 0) {
                            continue col; // not the end of the char
                        }
                    }
                    widths[y][x] = u;
                    continue char;
                }
                widths[y][x] = charSize.x;
            }
        }

        let name = file.replace(/\.png$/, "");
        fs.writeFileSync(`${path}/${name}.size`, widths.map(r => r.join(",")).join(",\n"));
    }
})();
