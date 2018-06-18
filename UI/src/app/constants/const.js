export const API_URL = document.ledPanelUiConfig.host

export const MIN_MODE_COLUMNS = 1
export const MAX_MODE_COLUMNS = 30
export const MIN_MODE_ROWS = 1
export const MAX_MODE_ROWS = 30

import {fillIncrementArray} from '../util/util'
export const MODE_COLUMN_VALUES = fillIncrementArray(MAX_MODE_COLUMNS - MIN_MODE_COLUMNS, MIN_MODE_COLUMNS)
export const MODE_ROW_VALUES = fillIncrementArray(MAX_MODE_ROWS - MIN_MODE_ROWS, MIN_MODE_ROWS)


