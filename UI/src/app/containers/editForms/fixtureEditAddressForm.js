import React, {Component} from 'react'
import {bindActionCreators} from 'redux'
import {connect} from 'react-redux'
import Modal from 'react-bootstrap4-modal' 
import { fixtureEditAddressFormReducer, formErrorReducer } from '../../reducers/formReducers'
import { closeEditFixtureAddressForm, riseFormValueError, clearFormValueError } from '../../actions/formActions'
import { editFixtureAddress } from '../../actions/stateActions'
import { 
    isInteger,
    fillIncrementArray,
    validateInteger
 } from '../../util/util'
import store from '../../store'
import {
    formModal,
    rowItem,
    textInputItem,
    contentItem,
    selectItem,
    radioItem,
    errorItem
} from './formItems'


const FORM_ID_PORT_NET = "EDIT_FIXTURE_FORM_ID_PORT_NET"
const FORM_ID_PORT_SUB = "EDIT_FIXTURE_FORM_ID_PORT_SUB"
const FORM_ID_PORT_UNI = "EDIT_FIXTURE_FORM_ID_PORT_UNI"
const FORM_ID_PORT_ADR = "EDIT_FIXTURE_FORM_ID_PORT_ADR"
const FORM_ID_PORT_UTIL_ADR = "EDIT_FIXTURE_FORM_ID_PORT_UTIL_ADR"
const FORM_ID_ADR_INCREM = "EDIT_FIXTURE_FORM_ID_ADR_INCREM"



class FixtureEditAddressForm extends Component {

    constructor(props){
        super(props) 
        this.incrment = true
    }

    handleClick() {
        this.props.click.clicked = true
    }    

    onClose(){
        this.props.closeEditFixtureAddressForm(this)
    }

    validate(validatons) {
        let res = "";
        validatons.map((v) => { if(!v.valid) res = res + v.error + " ||| " })
        return res;
    }

    onSubmit() {

        let validatons = [];
        let ids = store.getState().selectionReducer.fixtures
        
        let data = {
            net : this.getValue(FORM_ID_PORT_NET),
            sub : this.getValue(FORM_ID_PORT_SUB),
            uni : this.getValue(FORM_ID_PORT_UNI),
            addr : this.getValue(FORM_ID_PORT_ADR),
            utilAddr : this.getValue(FORM_ID_PORT_UTIL_ADR),
            increment : this.incrment
        }
        

        validatons.push(validateInteger(1,512, "Address", data.addr))
        validatons.push(validateInteger(1,512, "Util address", data.utilAddr))
        let res = this.validate(validatons)

        if(res !== "") {
            // we have form value error(s)
            this.props.riseFormValueError(res)
            return;
        }
        
        this.props.editFixtureAddress(ids, data)
        this.props.clearFormValueError()
        this.props.closeEditFixtureAddressForm(this)
    }



    getValue(id){
        return document.getElementById(id).value
    }

    
    render() {
        let form = this.props.form
        let portVals = fillIncrementArray(16, 0)

        return formModal(
            form.opened,
            "Edit fixture address",
            'Edit',
            'Cancel',
            this,
            () => contentItem([
                rowItem([
                    errorItem(this.props.error != null, this.props.error)
                ]),
                rowItem([
                    selectItem('Net', FORM_ID_PORT_NET, portVals),
                    selectItem('SubNet', FORM_ID_PORT_SUB, portVals),
                    selectItem('Universe', FORM_ID_PORT_UNI, portVals)
                ]),
                rowItem([
                    textInputItem('Address', FORM_ID_PORT_ADR, 1),
                    textInputItem("Util address", FORM_ID_PORT_UTIL_ADR, 512)
                ]),
                rowItem([
                    radioItem(
                        'Address auto increment',
                        FORM_ID_ADR_INCREM,
                        this.incrment
                    )       
                ])
            ])
        )
    }
}

const mapStateToProps = (state) => {
    return {
        form: state.fixtureEditAddressFormReducer,
        error: state.formErrorReducer
    }
}

const mapDispatchToProps = (dispatch) => {
    return bindActionCreators({
        editFixtureAddress: editFixtureAddress,
        closeEditFixtureAddressForm: closeEditFixtureAddressForm,
        riseFormValueError: riseFormValueError,
        clearFormValueError: clearFormValueError
    }, dispatch)
}

export default connect(mapStateToProps, mapDispatchToProps)(FixtureEditAddressForm)