import React, {Component} from 'react'
import {bindActionCreators} from 'redux'
import {connect} from 'react-redux'
import Modal from 'react-bootstrap4-modal' 
import { 
    storeStateFormReducer, 
    formErrorReducer,
    fileSelectedReducer
} from '../reducers/formReducers'
import { 
    closeStoreStateForm,
    riseFormValueError,
    selectFile, 
} from '../actions/formActions'
import { savedStatesReducer } from '../reducers/stateReducer'
import { saveState } from '../actions/stateActions'
import { fileList } from './justItems/randomItems'
import { rowItem, contentItem, textInputItem, errorItem, formModal } from './editForms/formItems'
import { ItemInfoRow } from '../components/itemInfoRow';

const FORM_ID_NAME = "FORM_STORE_ID_NAME"

class StoreStateForm extends Component {

    handleClick() {
        this.props.click.clicked = true
    }

    onClose(){
        this.props.closeStoreStateForm(this)
    }

    onSubmit() {
        const fn = this.getValue(FORM_ID_NAME)
        if(!(fn.length > 0)) {
            this.riseFormValueError("File name not set!")
        }
        this.props.saveState(fn)
        this.props.closeStoreStateForm(this)
    }

    getValue(id){
        return document.getElementById(id).value
    }

    render() {
        let form = this.props.form
        console.log("this.props.fileSelected");
        console.log(this.props.fileSelected);
        return(
            formModal(
                form.opened,
                "Save the day, save the project!",
                "Make it happen",
                "Whatever",
                this,
                () => contentItem([
                    errorItem(form.opened, this.props.error),
                    rowItem([fileList(this.props.files)]),
                    rowItem([
                        textInputItem(
                            "File name", 
                            FORM_ID_NAME,
                            this.props.fileSelected != null ?
                            this.props.fileSelected.filename : 
                            "hello"
                        )
                    ])
                ])
            )
        )
    }
}

const mapStateToProps = (state) => {
    return {
        form: state.storeStateFormReducer,
        error: state.formErrorReducer,
        fileSelected: state.fileSelectedReducer,
        files: state.savedStatesReducer
    }
}

const mapDispatchToProps = (dispatch) => {
    return bindActionCreators({
        saveState: saveState,
        selectFile: selectFile,
        closeStoreStateForm: closeStoreStateForm,
        riseFormValueError: riseFormValueError
    }, dispatch)
}

export default connect(mapStateToProps, mapDispatchToProps)(StoreStateForm)