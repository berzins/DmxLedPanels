
const MODE_GTL = "GridTopLeft"
const MODE_RBOI = "RectBorderOutIn"
const PATCH_SCTL = "pixelPatchSnakeColumnWiseTopLeft"
const PATCH_SRTL = "pixelPatchSnakeRowWiseTopLeft"
const PATCH_SCBL = "pixelPatchSnakeColumnWiseBottomLeft"
const PATCH_SRBR = "pixelPatchSnakeRowWiseBottomRight"
const PATCH_SCTR = "pixelPatchSnakeColumnWiseTopRight"

export class FixtureMode {
    static all() {
        return [MODE_GTL, MODE_RBOI]
    }

    static allShort() {
        return FixtureMode.all().map((m, i) => {return getShortNameFixtureMode(m)})
    }
}

export class FixturePatch {
    static all() {
        return [PATCH_SCTL, PATCH_SRTL, PATCH_SCBL, PATCH_SRBR, PATCH_SCTR]
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
        case MODE_RBOI: {
            return "RBOI " + mode.Params[0] + " zns";
        }
    }
    return "short name not set"
}

export const getShortNameFixturePatch = (patch) => {
    switch(patch.Name) {
        case PATCH_SCTL: {
            return "SCTL " + patch.Columns + "x" + patch.Rows
        }
        case PATCH_SRTL: {
            return 'SRTL ' + patch.Columns + 'x' + patch.Rows
        }
        case PATCH_SCBL: {
            return 'SCBL ' + patch.Columns + 'x' + patch.Rows
        }
        case PATCH_SRBR: {
            return 'SRBR ' + patch.Columns + 'x' + patch.Rows
        }
        case PATCH_SCTR: {
            return 'SCTR' + patch.Columns + 'x' + patch.Rows
        }
    }
    return "short name not set"
}

export const getPortString = (port) => {
    return port.Net + "." + port.SubNet + "." + port.Universe
}

// ---------------------

export const isInteger = (n) => {
    return !isNaN(n) && (function(x) { return (x | 0) === x; })(parseFloat(n))
}

export const fillIncrementArray = (size, start) => {
    let vals = Array.apply(null, Array(size))
    return vals.map((x, i) => {return i + start})
}

// @param start : first valid value
// @param end : last valid value
// @param name : string what describes value
// @param value : value to validate
// @return : object containing two props.. 
// boolean 'valid' : inicates if value is valid
// string 'error' : if not valid this property is populated with error information.
// othervise it is null.
export const validateInteger = (start, end, name, value) => {
    if(!isInteger(value)) {
        return{valid: false, error: name + " value is not an integer"}
    }
    if(value > end || value < start) {
        return {valid: false, error: name + " value should be in a range (" + start +" ... " + end + ")"}
    }
    return {valid: true, error: null};
}


export const getArrayIndexByValue = (array, value) => {
    let index = null
    let BreakException = {}
    try {
        array.forEach((item, i) => {
            if(item == value) {
                index = i
                throw BreakException   
            }
        })
    } catch (e) {
        if(e != BreakException) throw e
    }
    return index
}

export const patchSizeValues = [1, 6, 9, 11, 28, 56]




