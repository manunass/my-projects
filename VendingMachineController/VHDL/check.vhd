library ieee;
use ieee.std_logic_1164.all;
use ieee.numeric_std.all;

entity check_cash is
port(
balance, price: in std_logic_vector(4 downto 0);
coins : in std_logic_vector(3 downto 0);
bills : in std_logic_vector(3 downto 0);
change, enough : out std_logic);
end check_cash;


architecture checkarch of check_cash is
signal result: std_logic_vector(4 downto 0); 
begin
result <= std_logic_vector(signed(balance)-signed(price));
if result(4)='1' then
change <= '0' , enough<= '0';
end check_cash;