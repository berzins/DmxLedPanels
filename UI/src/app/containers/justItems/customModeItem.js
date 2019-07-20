import React,  {Component} from 'react'
import { rowItem, selectItem, radioItem, buttonItem } from '../editForms/formItems'
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

    // basically got to mode editor
    edit() {
        console.log("Custom mode edit called")
    }

    render() {
        const mode = this.props.mode
        const mi = this.props.modeIndex
        return(
           rowItem([
                rowItem([
                    selectItem("Type", createModeId(MODE_TYPE_ID, mi), FixtureMode.all(), mode.typeIndex),
                    radioItem('Select', createModeId(MODE_SELECT_ID, mi), false),
                    buttonItem("replace this", "Edit", this.edit.bind(this))
                ], true),
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