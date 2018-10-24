import React,  {Component} from 'react'
import { rowItem, selectItem, radioItem } from '../editForms/formItems'
import { FixtureMode, fillIncrementArray } from '../../util/util'

export const MODE_TYPE_ID = "MODE_TYPE_ID"
export const MODE_COLUMNS_ID = "MODE_COLUMNS_ID"
export const MODE_ROWS_ID = "MODE_ROWS_ID"
export const MODE_SELECT_ID = "MODE_SELECT_ID"

export default class ModeItem extends Component {


    render() {
        const sizeValues = fillIncrementArray(30, 1)
        const mode = this.props.mode
        const mi = this.props.modeIndex
        return(
           rowItem([
                rowItem([
                    selectItem("Type", createModeId(MODE_TYPE_ID, mi), FixtureMode.all(), mode.typeIndex),
                    radioItem('Select', createModeId(MODE_SELECT_ID, mi), false)
                ], true),
                rowItem([
                    selectItem("Columns" , createModeId(MODE_COLUMNS_ID, mi), sizeValues, mode.colIndex ),
                    selectItem("Rows" , createModeId(MODE_ROWS_ID, mi), sizeValues, mode.rowIndex),
                ])
           ])
        ) 
    }
}

export const createModeId = (id, i) => {
    return '' + id + i
}

export const getIndexFromModeId = (id) => {
    return id.replace(id, '')
}