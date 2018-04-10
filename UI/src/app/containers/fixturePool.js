import React, { Component } from 'react';
import Fixture from './fixture'
import AddFixtureButton from './addFixtureButton'


class FixturePool extends Component {

    createFixtureItems(count){
        let fix = []
        for(let i = 0; i < count; i++) {
            fix.push(
                <div className=" 
                    col-6
                    col-lg-4">
                        <Fixture />
                    </div>
            )
        }
        return fix
    }

    render() {
        return(
            <div className="output border border-danger">
                <div className="row">
                    <div className="col-12">
                        <AddFixtureButton/>
                    </div>
                    <div className="col-12">
                        <div className="row">
                            {this.createFixtureItems(20)}
                        </div>
                    </div>
                </div>      
            </div>
        );
    }
}

export default FixturePool;