
import { MODE_COLUMN_VALUES, MODE_ROW_VALUES} from '../constants/const'
import { FixtureMode } from './util'

export const convertModeIndexesToModevalues = modes => {
    return modes.map(m => {
        return {
            typeValue: FixtureMode.all()[m.typeIndex],
            colValue: MODE_COLUMN_VALUES[m.colIndex],
            rowValue: MODE_ROW_VALUES[m.rowIndex]
        }
    })
}