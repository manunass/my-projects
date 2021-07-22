import React,{useState,useEffect, Title} from 'react'
import {Table,Form} from 'react-bootstrap'

export default function Bagscan() {
    const [data, setdata] = useState([]);

    useEffect(() => {
        let data = [
            {id:1,name:"Hiba", status: "false"},
            {id:2,name:"Rayan", status: "false"},
            {id:3,name:"Abed", status: "false"}
        ];
        setdata(
            data.map(d => {
              return {
                select: false,
                id: d.id,
               name:d.name,
               status:d.status
                
              };
            })
          );
        }, []);
        const handleUpdate =(e) =>{
            e.preventDefault();
            console.log("submit");
            setdata(
                data.map(data => {
                  if (data.select===true) {
                    data.status="true";
                    console.log(data.status);
                  }
                  return data;
                })
              );
    
        }
       
    return (
        
        <div>
          <Title>Bag Scans</Title>
           <Form onSubmit={handleUpdate}>
            <Table striped bordered hover title="Bag Scans">
            <thead>
                <tr>
                <th>   <input
                type="checkbox"
                onChange={e => {
                  let checked = e.target.checked;
                  setdata(
                    data.map(d => {
                      d.select = checked;
                      return d;
                    })
                  );
                }}
              ></input>
              </th>
                <th>First Name</th>
                <th>Status</th>
                
                </tr>
            </thead>
            <tbody>

            {data.map((d, i) => (
            <tr key={d.id}>
              <th scope="row">
                <input
                  onChange={event => {
                    let checked = event.target.checked;
                    setdata(
                      data.map(data => {
                        if (d.id === data.id) {
                          data.select = checked;
                          console.log(data.select)
                        }
                        return data;
                      })
                    );
                  }}
                  type="checkbox"
                  checked={d.select}
                ></input>
              </th>
              <td>{d.name}</td>
              <td>{d.status}</td>
              
            </tr>
          ))}
            </tbody>
                
               
        </Table>
        <div className="text-center py-4 mt-3">
             <button className="btn btn-primary" type="submit">Update Status</button>
         </div>
      </Form>
        </div>
    )
}
