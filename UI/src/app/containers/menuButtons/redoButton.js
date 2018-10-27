import React, { Component } from 'react'
import { bindActionCreators } from 'redux'
import { connect } from 'react-redux'
import { redoState } from '../../actions/stateActions'

class ButtonRedoState extends Component {

    handleClick() {
        this.props.click.clicked = true
        this.props.redoState()
    }

    render() {
        return(
            <div
            style={this.props.style}
            className={"button btn util-button btn-primary btn-margin-right "}
            autoComplete="off"
            onClick={ () => this.handleClick()}>
                <div><h3><b>Redo</b></h3></div>
            </div>
        );
    }
}


const mapDispatchToProps = (dispatch) => {
    return bindActionCreators({
        redoState: redoState
    }, dispatch)
}

export default connect(null,mapDispatchToProps)(ButtonRedoState);