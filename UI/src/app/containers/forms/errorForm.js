import React, {Component} from 'react'
import {bindActionCreators} from 'redux'
import {connect} from 'react-redux'

import {
    formModal,
    labelItem,
    rowItem,
    buttonItem,
    contentItem,
    errorItem,
    textItem
} from '../editForms/formItems'

import { closeErrorForm } from '../../actions/formActions'


class ErrorForm extends Component {

    onClose() {
        this.props.closeErrorForm()
    }

    onSubmit() {
        this.props.closeErrorForm()
    }

    handleClick() {
        this.props.click.clicked = true
    }

    render() {
        return(
            formModal(
                this.props.error.visible,
                "Error",
                "OK",
                "Cancel",
                this,
                () => {
                    return contentItem([
                        rowItem([
                            textItem(this.props.error.msg)
                        ])
                    ])
                } 
            )
        )
    }
}

const mapStateToProps = (state) => {
    return {
        error: state.errorReducer
    }
}

const mapDispatchToProps = (dispatch) => {
    return bindActionCreators({
        closeErrorForm: closeErrorForm
    }, dispatch)
}

export default connect(mapStateToProps, mapDispatchToProps) (ErrorForm)