import React, {Component} from 'react'
import {bindActionCreators} from 'redux'
import {connect} from 'react-redux' 
import { outputEditNameFormReducer } from '../../reducers/formReducers'
import { closeEditOutputNameForm } from '../../actions/formActions'
import { editOutputName } from '../../actions/stateActions'
import { 
    rowItem,
    textInputItem,
    formModal
} from './formItems'
import { fillIncrementArray } from './formUtil'
import store from '../../store'

const FORM_ID_NAME = "FORM_EDOT_OUTPUT_ID_NAME"

class OutputEditNameForm extends Component {

    handleClick() {
        this.props.click.clicked = true
    }

    onClose(){
        this.props.closeEditOutputNameForm(this)
    }

    onSubmit() {
        const data = {
            name : this.getValue(FORM_ID_NAME)
        }
        const ids = store.getState().selectionReducer.outputs

        this.props.editOutputName(ids, data)
        this.props.closeEditOutputNameForm(this)
    }

    getValue(id){
        return document.getElementById(id).value
    }

    render() {
        let form = this.props.form
        return(
            formModal(
                form.opened, 
                "Edit output name", 
                "Edit", 
                "Cancel", 
                this,
                () => rowItem([
                    textInputItem("Name", FORM_ID_NAME, "Output")
                ])
            )
        )
    }
}

const mapStateToProps = (state) => {
    return {
        form: state.outputEditNameFormReducer,
    }
}

const mapDispatchToProps = (dispatch) => {
    return bindActionCreators({
        editOutputName: editOutputName,
        closeEditOutputNameForm: closeEditOutputNameForm
    }, dispatch)
}

export default connect(mapStateToProps, mapDispatchToProps)(OutputEditNameForm)