import React, { Component } from 'react'
import { bindActionCreators } from 'redux'
import { connect } from 'react-redux'
import { openStoreStateForm } from '../actions/formActions'

class SaveButton extends Component {

    handleClick(){
        this.props.click.clicked = true
        this.props.openStoreStateForm(null)
    }

    render() {
        return(
            <div
            style={this.props.style}
            className={"button btn btn-dark util-button btn-margin-right"}
            autoComplete="off"
            onClick={ () => this.handleClick() }
            >
                <div><h3><b>Save</b></h3></div>
            </div>
        );
    }
}

const mapStateToProps = (state) => {
    return {

    }
}

const mapDispatchToProps = (dispatch) => {
    return bindActionCreators({
        openStoreStateForm: openStoreStateForm
    }, dispatch)
}

export default connect(mapDispatchToProps, mapDispatchToProps)(SaveButton)