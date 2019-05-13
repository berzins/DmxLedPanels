const fillIncrementArray = (size, start) => {
    let vals = Array.apply(null, Array(size))
    return vals.map((x, i) => {return i + start})
}


export const API_URL = document.ledPanelUiConfig.host


export const MIN_MODE_VALUE = 1
export const MAX_MODE_VALUE = 56
export const MIN_MODE_COLUMNS = MIN_MODE_VALUE
export const MAX_MODE_COLUMNS = MAX_MODE_VALUE
export const MIN_MODE_ROWS = MIN_MODE_VALUE
export const MAX_MODE_ROWS = MAX_MODE_VALUE


export const PATCH_ROW_VALUES = fillIncrementArray(MAX_MODE_VALUE - MIN_MODE_VALUE, MIN_MODE_VALUE)
export const PATCH_COLUMN_VALUES = fillIncrementArray(MAX_MODE_VALUE - MIN_MODE_VALUE, MIN_MODE_VALUE)
export const MODE_COLUMN_VALUES = PATCH_COLUMN_VALUES
export const MODE_ROW_VALUES = PATCH_ROW_VALUES
export const MODE_VALUES = fillIncrementArray(MAX_MODE_VALUE - MIN_MODE_VALUE, MIN_MODE_VALUE)

