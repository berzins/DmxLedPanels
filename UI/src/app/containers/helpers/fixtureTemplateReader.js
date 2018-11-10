import { 
    FixturePatch,
    patchSizeValues,
    fillIncrementArray,
    FixtureMode,
} from '../../util/util'


export const getFixtureTemplateData = template => {
    return {
        name: template.Name,
        pixelPatch: {
            name: getValuesIndexPair(FixturePatch.all(), template.PixelPatch.Name),
            columns: getValuesIndexPair(patchSizeValues, template.PixelPatch.Columns),
            rows: getValuesIndexPair(patchSizeValues, template.PixelPatch.Rows)
        },
        modes: getModesValues(template.Modes),
        modeIndex: template.CurrentModeIndex,
        address: getAddressValues(template.Address),
        utilAddress: getAddressValues(template.UtilAddress),
        utilsEnabled: template.UtilsEnabled,
    }
}

export const getValuesIndexPair = (values, value) => {
    let index =  values.findIndex(v => v == value);
    if(index < 0) {
        index = 0
    }
    return {
        values: values,
        index: index
    }
}

const protValues = fillIncrementArray(16, 0)

const getAddressValues = (address) => {
    return {
        port: {
            net: getValuesIndexPair(protValues, address.Port.Net),
            subNet: getValuesIndexPair(protValues, address.Port.SubNet),
            universe: getValuesIndexPair(protValues, address.Port.Universe)
        },
        dmxAddress: address.DmxAddress
    }
}

import {MODE_VALUES} from '../../constants/const'

export const getModesValues = (modes) => {
    return modes.map(m => {return getModeValues(m)})
}

const getModeValues = (mode) => {
    return {
        name: getValuesIndexPair(FixtureMode.all(), mode.Name),
        params: mode.Params.map(p => {return getValuesIndexPair(MODE_VALUES, p)})        
    }
}

