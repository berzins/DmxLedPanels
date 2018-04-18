import React, {Component} from 'react'
import {bindActionCreators} from 'redux'
import {connect} from 'react-redux' 
import { outputEditPortFormReducer } from '../../reducers/formReducers'
import { closeEditOutputPortForm } from '../../actions/formActions'
import { editOutputPort } from '../../actions/stateActions'
import { 
    rowItem,
    selectItem,
    formModal
} from './formItems'
import { fillIncrementArray } from './formUtil'
import store from '../../store'

const FORM_ID_PORT_NET = "FORM_EIDI_OUT_ID_PORT_NET"
const FORM_ID_PORT_SUB = "FORM_EIDI_OUT_ID_PORT_SUB"
const FORM_ID_PORT_UNI = "FORM_EIDI_OUT_ID_PORT_UNI"

class OutputEditPortForm extends Component {

    handleClick() {
        this.props.click.clicked = true
    }

    onClose(){
        this.props.closeEditOutputPortForm(this)
    }

    onSubmit() {
        const data = {
            net : this.getValue(FORM_ID_PORT_NET),
            sub : this.getValue(FORM_ID_PORT_SUB),
            uni : this.getValue(FORM_ID_PORT_UNI)
        }
        const ids = store.getState().selectionReducer.outputs

        this.props.editOutputPort(ids, data)
        this.props.closeEditOutputPortForm(this)
    }

    getValue(id){
        return document.getElementById(id).value
    }

    render() {
        const form = this.props.form
        const portVals = fillIncrementArray(16, 0)
        return(
            formModal(
                form.opened, 
                "Edit output name", 
                "Edit", 
                "Cancel", 
                this,
                () => rowItem([
                    selectItem("Net", FORM_ID_PORT_NET, portVals),
                    selectItem("SubNet", FORM_ID_PORT_SUB, portVals),
                    selectItem("Universe", FORM_ID_PORT_UNI, portVals)
                ])
            )
        )
    }
}

const mapStateToProps = (state) => {
    return {
        form: state.outputEditPortFormReducer,
    }
}

const mapDispatchToProps = (dispatch) => {
    return bindActionCreators({
        editOutputPort: editOutputPort,
        closeEditOutputPortForm: closeEditOutputPortForm
    }, dispatch)
}

export default connect(mapStateToProps, mapDispatchToProps)(OutputEditPortForm)