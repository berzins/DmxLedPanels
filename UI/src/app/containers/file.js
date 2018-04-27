import React, { Component } from 'react'
import { bindActionCreators } from 'redux'
import { connect } from 'react-redux'
import { selectFile } from '../actions/formActions'

class File extends Component {


    handleClick(){
        this.props.selectFile(this.props.filename, this.props.parentId)
    }

    render() {
        return (
            <a 
            href="#"
            className="list-group-item list-group-item-action" 
            id={this.props.filename}
            onClick={() => this.handleClick()}
            >
                {this.props.filename}
            </a>
        )
    }
}

const mapDispatchToProps = dispatch => {
    return bindActionCreators(
        {
            selectFile:selectFile
        }, dispatch)
}

export default connect(null, mapDispatchToProps)(File)


