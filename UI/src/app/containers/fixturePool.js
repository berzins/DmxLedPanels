import React, { Component } from 'react'
import { bindActionCreators } from 'redux'
import { connect } from 'react-redux'
import Fixture from './fixture'
import AddFixtureButton from './addFixtureButton'
import DeselectButton from './deselectButton'
import DeleteFixtureButton from './deleteFixtureButton'
import { unpatchFixture } from '../actions/stateActions'
import { stateReducer } from '../reducers/stateReducer'
import  store  from '../store'



class FixturePool extends Component {

    constructor(props) {
        super(props)
        this.poolClick = {
            id: null,
            clicked: false
        }
    }


    isSelected(id) {
        let ret = false
        store.getState().selectionReducer.fixtures
        .forEach(i => { if(id === i) ret = true})
        return ret
    }

    renderFixtures(fixtures) {
        return fixtures.map((fix, i) => {
            return(
                <div key={"fix" + fix.Id} 
                className=" 
                col-6
                col-lg-4">
                    <Fixture fixture={fix} click={this.poolClick} />
                </div>
            )
        })
    }

    handleClick() {
        // handle global click
        this.props.click.clicked = this.poolClick.clicked

        // handle fixture pool click
        if(this.poolClick.clicked) {

            this.poolClick.clicked = false
        } else {


            let ids = []
            this.props.patchedFix
            .forEach(f => {
                if(f.PatchedTo !== -1 && this.isSelected(f.Id)) 
                ids.push(f.Id)
            })

            if(ids.length > 0) 
                this.props.unpatchFixture(ids)
        }
    }

    render() {
        return(
            <div 
            className="output"
            onClick={() => this.handleClick()}
            >
                <div className="row">
                    <div className="col-12 btn-group">
                        <AddFixtureButton click={this.props.click}/>
                        <DeleteFixtureButton click={this.props.click}/>
                    </div>
                    <div className="col-12">
                        <div className="row">
                            {this.renderFixtures(this.props.fixtures)}
                        </div>
                    </div>
                </div>      
            </div>
        );
    }
}

const mapStateToProps = (state) => {
    return {
        
    }
}

const mapDispatchToProps = (dispatch) => {
    return bindActionCreators(
        {
            unpatchFixture: unpatchFixture
        }, dispatch)
}

export default connect(mapStateToProps, mapDispatchToProps)(FixturePool)