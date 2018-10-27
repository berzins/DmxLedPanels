import React,  {Component} from 'react'
import {bindActionCreators} from 'redux'
import {connect} from 'react-redux'
import {
    formModal,
    labelItem,
    rowItem,
    contentItem,
    buttonItem,
    textInputItem,
} from '../editForms/formItems'

import { modeManagerFormReducer } from '../../reducers/formReducers'
import { closeModeManagerForm, submitModes } from '../../actions/formActions'
import {
    updateModes
} from '../../actions/formActions'
import ModeItem, 
{ 
    MODE_TYPE_ID,
    MODE_COLUMNS_ID,
    MODE_ROWS_ID,
    MODE_SELECT_ID,
    createModeId
} from '../justItems/modeItem'

const ACTION_UPDATE = 'UPDATE'
const ACTION_ADD = 'ADD'
const ACTION_RMEOVE = 'REMOVE'

class ModeManagerForm extends Component {

    onClose(){
        this.props.closeModeManagerForm(this)
    }

    onSubmit() {
        this.props.updateModes(this.getModeIndexValues())
        setTimeout(() => {
            this.props.submitModes(this.props.formState.modes)
            this.props.closeModeManagerForm(this)
        }, 200)
    }

    handleClick() {
        this.props.click.clicked = true
    }

    onAddMode() {
        const mIndexes = this.getModeIndexValues()
        mIndexes.push({
            typeIndex: 0,
            colIndex: 2,
            rowIndex: 2
        })
        this.props.updateModes(mIndexes)
    }

    onRemoveMode() {       
        const modes = []
        const mIndexes = this.getModeIndexValues()
        mIndexes.forEach((m) => {
            console.log(m.selected)
            if(m.selected != true) {
                modes.push(m)
            }
        })
        this.props.updateModes(modes)
    }

    getModeValues() {
        return this.props.formState.modes.map((m, i) => {
            return {
                ...m,
                typeValue: document.getElementById(createModeId(MODE_TYPE_ID, i)).value,
                colValue: document.getElementById(createModeId(MODE_COLUMNS_ID, i)).value,
                rowValue: document.getElementById(createModeId(MODE_ROWS_ID, i)).value,
                selected: document.getElementById(createModeId(MODE_SELECT_ID, i)).checked
            }
        })
    }

    getModeIndexValues() {

        return this.props.formState.modes.map((m, i) => {
            return {
                ...m,
                typeIndex: document.getElementById(createModeId(MODE_TYPE_ID, i)).selectedIndex,
                colIndex: document.getElementById(createModeId(MODE_COLUMNS_ID, i)).selectedIndex,
                rowIndex: document.getElementById(createModeId(MODE_ROWS_ID, i)).selectedIndex,
                selected: document.getElementById(createModeId(MODE_SELECT_ID, i)).checked
            }
        })
    }

    updateCurrentModes(action) {
        switch(action) {
            case ACTION_UPDATE: {}
        }
    }

    renderMode(mode, i) {
        return(
            <ModeItem mode={mode} modeIndex={i} key={createModeId(MODE_TYPE_ID, i)}/>
        )
    }

    renderModes(modes) {
        return modes.length != 0 ? modes.map((m, i) => {return this.renderMode(m, i)}) : <div key="asdassgdh"></div>
    }

    render() {
        return(
            formModal(
                this.props.formState.visible,
                "Mode manager",
                "Cool",
                "Return to previous dimension",
                this,
                () => {
                    return contentItem([
                        rowItem([
                            buttonItem("ADD_MODE_BUTTON_", "Add", this.onAddMode.bind(this)),
                            buttonItem("REMOVE_MODE_BUTTON", "Remove", this.onRemoveMode.bind(this))
                        ]),
                        contentItem([
                            this.renderModes(this.props.formState.modes)
                        ])
                    ])
                }
            )
        )
    }
}

const mapStateToProps = (state) => {
    return {
        formState: state.modeManagerFormReducer
    }
}

const mapDispatchToProps = (dispatch) => {
    return bindActionCreators({
        updateModes: updateModes,
        closeModeManagerForm: closeModeManagerForm,
        submitModes: submitModes
    }, dispatch)
}

export default connect(mapStateToProps, mapDispatchToProps) (ModeManagerForm)