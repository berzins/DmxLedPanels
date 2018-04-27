import React, {Component} from 'react'
import {bindActionCreators} from 'redux'
import {connect} from 'react-redux'
import Modal from 'react-bootstrap4-modal' 

import { 
    fileSelectedReducer, 
    loadStateFormReducer , 
    formErrorReducer 
} from '../reducers/formReducers'

import { 
    closeOpenStateForm, 
    riseFormValueError, 
    selectFile 
} from '../actions/formActions'

import { errorItem } from './editForms/formItems'
import { loadStateFromFile, getSavedStates, getCurretProject } from '../actions/stateActions'
import { savedStatesReducer } from '../reducers/stateReducer'
import { fileList } from './justItems/randomItems'

class LoadStateForm extends Component {

    handleClick() {
        this.props.click.clicked = true
    }

    onClose(){
        //this.props.selectFile(null)
        this.props.closeOpenStateForm(this)
    }

    onSubmit() {
        
        if(this.props.selectedFile.filename == null) {
            this.props.riseFormValueError("File not selected!")
            return
        }
        this.props.loadStateFromFile(this.props.selectedFile.filename)
        setTimeout(() => {
            
            this.props.getCurretProject();
        }, 200)
        this.onClose()
    }

    renderSelectedFile() {
        if(this.props.selectedFile.filename != null) {
            return <div className="badge badge-success">{"selected file: '" + this.props.selectedFile.filename + "'" }</div>
        } 
        return <div className="badge badge-danger">{"Wazzzzaaaaaaap!" }</div>
    }

    componentDidMount() {
        this.props.getSavedStates()
    }

    render() {
        let form = this.props.form
        return(
            <Modal visible={form.opened} onClickBackdrop={() => this.onClose() } onClick={() => this.handleClick()}>
                <div className="modal-header">
                <h5 className="modal-title">{"Load project"}</h5>
                </div>
                <div className="modal-body">
                {errorItem(this.props.error != null, this.props.error)}
                <form>
                <div className="form-row align-items-center">

                    {fileList(this.props.files)}
                    {this.renderSelectedFile()}
                </div>
                </form>
                </div>
                <div className="modal-footer">
                <button type="button" className="btn btn-secondary" onClick={() => this.onClose() }>
                    Whaaat?
                </button>
                <button type="button" className="btn btn-primary" onClick={ () => this.onSubmit() }>
                    Pam pa ram OK.
                </button>
                </div>
            </Modal>
        )
    }
}

const mapStateToProps = (state) => {
    return {
        selectedFile: state.fileSelectedReducer,
        files: state.savedStatesReducer,
        error: state.formErrorReducer,
        form: state.loadStateFormReducer,
    }
}

const mapDispatchToProps = (dispatch) => {
    return bindActionCreators({
        closeOpenStateForm: closeOpenStateForm,
        riseFormValueError: riseFormValueError,
        loadStateFromFile: loadStateFromFile,
        getSavedStates: getSavedStates,
        selectFile: selectFile,
        getCurretProject: getCurretProject
    }, dispatch)
}

export default connect(mapStateToProps, mapDispatchToProps)(LoadStateForm)