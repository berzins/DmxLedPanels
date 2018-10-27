import React, { Component } from 'react'
import { bindActionCreators } from 'redux'
import { connect } from 'react-redux'
import { openLoadStateForm } from '../actions/formActions'

class LoadButton extends Component {

    handleClick(){
        this.props.click.clicked = true
        this.props.openLoadStateForm(this)
    }

    render() {
        return(
            <div
            style={this.props.style}
            className={"button btn btn-dark util-button btn-margin-right"}
            autoComplete="off"
            onClick={ () => this.handleClick() }
            >
                <div><h3><b>Load</b></h3></div>
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
        openLoadStateForm: openLoadStateForm
    }, dispatch)
}

export default connect(mapDispatchToProps, mapDispatchToProps)(LoadButton)