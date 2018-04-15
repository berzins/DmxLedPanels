import React, { Component } from 'react';
import { bindActionCreators } from 'redux'
import { connect } from 'react-redux'
import FixturePool from './fixturePool'
import OutputPool from './outputPool'
import UnpatchField from './unpatchField'
import { loadState } from '../actions/stateActions'

class ItemContainer extends Component {


    componentDidMount() {
        this.props.loadState()
        
    }

    handleClick() {
        this.props.click.clicked = true
    }



    renderContent() {
        if(this.props.state.data.Outputs == null) {
            return(
                <div>
                    sorry, nebūs
                </div>
            )
        }

        let outFix = []
        this.props.state.data.Outputs.forEach(o => {
            o.Fixtures.forEach(f => {outFix.push(f)})
        });
        return(
            <div onClick={() => this.handleClick()}>
            
            <div className="row">

                <div className="col-8">
                    <OutputPool 
                    click={this.props.click}
                    outputs={this.props.state.data.Outputs} />
                </div>
                <div className="col-4">
                    <FixturePool
                    click={this.props.click} 
                    fixtures={this.props.state.data.FixturePool}
                    patchedFix={outFix}
                    />
                </div>
                
                </div>
            </div>
        )
    }

    renderMessage(msg) {
        return(
            <div>
                <div className="row">
                    <div className="col-12">
                        <div className="jumbotron">{msg}</div>
                    </div>
                </div>
            </div>
        )
    }

    render() {
        if(this.props.state.loaded) {
            let error = this.props.state.error
            if(this.props.state.error != null){
                if(this.props.state.error.type !== 'connection-error') { return this.renderContent() }
                return this.renderMessage(error.type + " : " + error.content)
            } else {
                return this.renderContent()
            }
        } else {
            return this.renderMessage("Waiting data")
        }
    }
}


const mapStateToProps = (state) => {
    return {
        state: state.stateReducer
    }
}

const mapDispatchToProps = (dispatch) => {
    return bindActionCreators({
        loadState: loadState,
    }, dispatch)
}

export default connect(mapStateToProps, mapDispatchToProps)(ItemContainer)