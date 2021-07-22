import React,{useState,useEffect} from 'react'
import {Col,Table,Row,Form} from 'react-bootstrap'
import { MDBCard, MDBBtn} from
'mdbreact'
import {baseUrl} from '../../package.json'

import axios from 'axios'



export default function Profile(props) {
    const [data, setdata] = useState(' ')
    const [BatchID, setBatchID] = useState('')
  const [Batch, setBatch] = useState({Id: "",MunicipalityId: "",RecyclableId: "",Recyclable: {
    Id: "",
    Material: "",
    LbpPerKg: 0
  },
  BatchNumber: 0,
  Revenue: 0,
  Current: true
})
  const [bagscan, setbagscan] = useState({ Id: "", UserId: "", EmployeeId: "", BatchId: "", Weight: 0, Processed:false, UnixTimeScanned: 0,  UnixTimeProcessed: 0 })
  
  const [update, setupdate] = useState(true)
  useEffect( () => {
    
    const getBatches = async () => {
        
        try {
           
      const batches = await axios.get(baseUrl+'/api/Operations/0713b854-89ef-497b-a61c-e0bdb669fa70/recyclables/'+props.p+'/batches')
          console.log(props.p)
          setBatch(batches.data);
          setBatchID(batches.data.Id);
         
            
        } catch (err) {
          console.error(err.message);
        }
      };
    
      getBatches();
      console.log(Batch)
      
   
  }, [props]);

  useEffect(() => {
    if(BatchID!==''){
        axios.get(baseUrl+'/api/Operations/recyclables/batches/'+BatchID+'/bag-scans').then(
            res=>{
                setbagscan(res.data.BagScans)
                console.log(res.data.BagScans)
                setupdate(false)
            }
        )
         
        }
  }, [BatchID]);
  const revenue ={
    Revenue:parseInt(data)
}
  const handleUpdate = async (event) => {
    event.preventDefault();
      console.log(data);
       await fetch(baseUrl+'/api/Operations/recyclables/batches/'+BatchID, {
        method: "PUT", headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(revenue)
      })
      alert("Revenue Updated")
      setdata('');
      setupdate(true)
    }
    const handleProcess=async (event) =>{
        event.preventDefault();
        console.log(BatchID)
        await fetch(baseUrl+'/api/Operations/recyclables/batches/'+BatchID, {
            method: "PATCH", headers: { 'Content-Type': 'application/json' }})
    }
  
    return (
      <div>
        <h3 align='center'><b>Bag Scans</b></h3>
        <Row className="m-1">
            <MDBCard  className="wrapper">
                <Table responsive>
                <thead>
                <tr>
                <th>User ID</th>
                <th>Batch ID</th>
                <th>Weight</th>
                <th>UnixTimeScanned</th>
                <th>UnixTimeProcessed</th>
                </tr>
               </thead>
               {!update ?
               <tbody>
                  
               {bagscan.map(bag =>{
                return(  
                <tr key={bag.Id}>
                      <td>{bag.UserId}</td>
                     <td>{bag.BatchId}</td>
                      <td>{bag.Weight}</td>
                   
                      <td>{bag.UnixTimeScanned}</td>
                      <td>{bag.UnixTimeProcessed}</td>   
                 </tr>
                 ) })} 
                
               </tbody>
                : " "}
                </Table>
                </MDBCard>
          </Row>
           <Row>
             <Col lg="5">
              </Col>
              <Col>
               <Form onSubmit={handleUpdate}>
              <MDBCard className="wrapper">
              <p className="h6 text-center py-4">Current Batch</p>
              <p className="ml-2">Batch ID: </p><p className="ml-2">{Batch.Id}</p>
              <p className="ml-2">Recyclable ID:</p><p className="ml-2"> {props.p}</p>
              <p className="ml-2">Revenue:</p><p className="ml-2"> {Batch.Revenue}</p>
              <label className="grey-text font-weight-light ml-3 " >Update Revenue: </label>
              <input type="text" className="form-control " value={data}  onChange={(e) =>setdata(e.target.value)} required/><br />
              <MDBBtn className="btn btn-primary" type="submit" size="sm">Update</MDBBtn>  
              </MDBCard>
             </Form>
             <Form onSubmit={handleProcess}>
              <MDBBtn className="btn btn-primary" type="submit" size="sm">Process</MDBBtn>  
             </Form>
             </Col>
       </Row>
       </div>
    )
}

