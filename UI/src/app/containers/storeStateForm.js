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
import { savedStatesReducer, currentProjectReducer } from '../reducers/stateReducer'
import { saveState, getCurretProject } from '../actions/stateActions'
import { fileList } from './justItems/randomItems'
import { rowItem, contentItem, textInputItem, errorItem, formModal, textItem } from './editForms/formItems'
import { ItemInfoRow } from '../components/itemInfoRow';

const FORM_ID_NAME = "FORM_STORE_ID_NAME"
const SELECTOR_THIS = "SELECTOR_THIS"

class StoreStateForm extends Component {

    constructor(props) {
        super(props);
        
    }

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

    getFileName() {
        const f = this.props.fileSelected 
        if(f.filename != null) {
            return f.selector == SELECTOR_THIS ? 
            f.filename : this.props.currentProjet.name
        }
        return this.props.currentProjet.name
    }

    // shouldComponentUpdate(nextProps, nextState) {
    //     return !this.props.form.opened && !nextProps.form.opened ?
    //     false : true

    // }

    componentDidMount() {
        this.props.getCurretProject();
    }

    render() {
        const form = this.props.form
        const filename = this.getFileName()
        console.log("this.props");
        console.log(this.props);
        return(
            formModal(
                form.opened,
                "Save the day, save the project!",
                "Make it happen",
                "Whatever",
                this,
                () => contentItem([
                    textItem(this.props.currentProjet.name),
                    errorItem(form.opened, this.props.error),
                    rowItem([fileList(this.props.files, SELECTOR_THIS)]),
                    rowItem([
                        textInputItem(
                            "File name", 
                            FORM_ID_NAME,
                            filename
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
        files: state.savedStatesReducer,
        currentProjet: state.currentProjectReducer
    }
}

const mapDispatchToProps = (dispatch) => {
    return bindActionCreators({
        saveState: saveState,
        selectFile: selectFile,
        closeStoreStateForm: closeStoreStateForm,
        riseFormValueError: riseFormValueError,
        getCurretProject: getCurretProject
    }, dispatch)
}

export default connect(mapStateToProps, mapDispatchToProps)(StoreStateForm)