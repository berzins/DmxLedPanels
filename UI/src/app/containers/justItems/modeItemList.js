import React, {Component} from 'react'
import {bindActionCreators} from 'redux'
import {connect} from 'react-redux'
import {contentItem, buttonItem, rowItem, textItem } from '../editForms/formItems'
import {openModeManagerForm} from '../../actions/formActions'
import { 
    FixtureMode,
} from '../../util/util'
import {MODE_COLUMN_VALUES, MODE_ROW_VALUES } from '../../constants/const'

class ModeItemList extends Component {


    getModeValues(modes) {
        return modes.map((m, i) => {
            return {
                ...m,
                typeValue: FixtureMode.all()[m.typeIndex],
                colValue: MODE_COLUMN_VALUES[m.colIndex],
                rowValue: MODE_ROW_VALUES[m.rowIndex]
            }
        })
    }

    renderMode(mode) {
        return textItem(
            "Type: " + mode.typeValue + ", " +
            "Columns: " + mode.colValue + ", " + 
            "Rows: " + mode.rowValue
        )
    }

    renderModes(modes) {
        return modes.map(mode => {return this.renderMode(mode)})
    }

    render() {
        console.log(this.props)
        return(
            contentItem([
                rowItem([
                    buttonItem("MANAGE_MODES_BUTTON", "Manage modes", () => {this.props.openModeManagerForm()}) 
                ]),
                contentItem([
                    this.renderModes(this.getModeValues(this.props.modes))
                ])
            ])
        )
    }
}

const mapStateToProps = state => {
    console.log(state.modesReducer)
    return {
        modes: state.modesReducer.modes
    }
}

const mapDispatchToProps = dispatch => {
    return bindActionCreators({
        openModeManagerForm: openModeManagerForm
    }, dispatch)
}


export default connect(mapStateToProps, mapDispatchToProps)(ModeItemList)
