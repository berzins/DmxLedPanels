/* eslint-disable */
import React,  {Component} from 'react'
import { rowItem, selectItem, radioItem } from '../editForms/formItems'
import { FixtureMode } from '../../util/util'
import { 
    MODE_COLUMN_VALUES,
    MODE_ROW_VALUES
} from '../../constants/const'


export const MODE_TYPE_ID = "MODE_TYPE_ID"
export const MODE_COLUMNS_ID = "MODE_COLUMNS_ID"
export const MODE_ROWS_ID = "MODE_ROWS_ID"
export const MODE_SELECT_ID = "MODE_SELECT_ID"

export default class ModeItem extends Component {


    render() {
        const mode = this.props.mode
        const mi = this.props.modeIndex
        return(
           rowItem([
                rowItem([
                    selectItem("Type", createModeId(MODE_TYPE_ID, mi), FixtureMode.all(), mode.typeIndex),
                    radioItem('Select', createModeId(MODE_SELECT_ID, mi), false)
                ], true),
                rowItem([
                    selectItem("Columns" , createModeId(MODE_COLUMNS_ID, mi), MODE_COLUMN_VALUES, mode.colIndex ),
                    selectItem("Rows" , createModeId(MODE_ROWS_ID, mi), MODE_ROW_VALUES, mode.rowIndex),
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