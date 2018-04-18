import React, {Component} from 'react'
import {bindActionCreators} from 'redux'
import {connect} from 'react-redux' 
import { outputEditIpFormReducer, formErrorReducer } from '../../reducers/formReducers'
import { 
    closeEditOutputIpForm, 
    riseFormValueError,
    clearFormValueError 
} from '../../actions/formActions'
import { editOutputIp } from '../../actions/stateActions'
import { 
    rowItem,
    textInputItem,
    formModal,
    contentItem,
    errorItem
} from './formItems'
import { fillIncrementArray, checkIpAddress, validate } from './formUtil'
import store from '../../store'

const FORM_ID_IP = "FORM_EDOT_OUTPUT_ID_IP"

class OutputEditIpForm extends Component {

    handleClick() {
        this.props.click.clicked = true
    }

    onClose(){
        this.props.closeEditOutputIpForm(this)
    }

    onSubmit() {
        const data = {
            ip : this.getValue(FORM_ID_IP)
        }
        const ids = store.getState().selectionReducer.outputs

        const validations = []
        validations.push(checkIpAddress(data.ip))
        const res = validate(validations);
        if(res != "") {
            this.props.riseFormValueError(res)
            return
        }

        this.props.editOutputIp(ids, data)
        this.props.clearFormValueError()
        this.props.closeEditOutputIpForm(this)
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
                () => contentItem([
                    rowItem([
                        errorItem(this.props.error != null, this.props.error)
                    ]),
                    rowItem([
                        textInputItem("IP address", FORM_ID_IP, "192.168.0.255")
                    ])
                ])
            )
        )
    }
}

const mapStateToProps = (state) => {
    return {
        form: state.outputEditIpFormReducer,
        error: state.formErrorReducer
    }
}

const mapDispatchToProps = (dispatch) => {
    return bindActionCreators({
        editOutputIp, editOutputIp,
        closeEditOutputIpForm: closeEditOutputIpForm,
        riseFormValueError: riseFormValueError,
        clearFormValueError: clearFormValueError
    }, dispatch)
}

export default connect(mapStateToProps, mapDispatchToProps)(OutputEditIpForm)