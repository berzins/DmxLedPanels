

const MODE_GTL = "GridTopLeft"
const PATCH_SCTL = "pixelPatchSnakeColumnWiseTopLeft"

export class FixtureMode {
    static all() {
        return [MODE_GTL]
    }

    static allShort() {
        return FixtureMode.all().map((m, i) => {return getShortNameFixtureMode(m)})
    }
}

export class FixturePatch {
    static all() {
        return [PATCH_SCTL]
    }

    static allShort() {
        return FixturePatch.all().map((p, i) => {return getShortNameFixturePatch(p)})
    }
}

export const getShortNameFixtureMode = (mode) => {
    switch(mode.Name) {
        case MODE_GTL: {
            return "GTL " + mode.Params[0] + "x" + mode.Params[1]
        }
    }
    return ">)(<"
}

export const getShortNameFixturePatch = (patch) => {
    switch(patch.Name) {
        case PATCH_SCTL: {
            return "SCTL " + patch.Columns + "x" + patch.Rows
        }
    }
    return ">)(<"
}

export const getPortString = (port) => {
    return port.Net + "." + port.SubNet + "." + port.Universe
}

// ---------------------

export const isInteger = (n) => {
    return !isNaN(n) && (function(x) { return (x | 0) === x; })(parseFloat(n))
}

