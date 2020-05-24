library ieee;
use ieee.std_logic_1164.all;
use ieee.numeric_std.all;

entity get_value is
port (
  clk : in std_logic;
  collect: in std_logic_vector(1 to 72);
  price : out std_logic_vector(4 downto 0));
end get_value;

architecture archi of get_value is 

begin

process(collect, clk)
begin
	if (collect(1) = '1' or collect(2) = '1' or collect(3) = '1' or collect(13) = '1' or collect(26) = '1' or collect(30) = '1' or collect(43) = '1' or collect(45) = '1' or collect(46) = '1' or collect(61) = '1' or collect(62) = '1' or collect(63) = '1' or collect(64) = '1' or collect(65) = '1' or collect(66) = '1' or collect(67) = '1' or collect(68) = '1' or collect(69) = '1' or collect(70) = '1' ) then
		price <= "00100";

	elsif (collect(4) = '1' or collect(5) = '1' or collect(6) = '1' or collect(16) = '1' or collect(28) = '1' or collect(29) = '1' or collect(37) = '1' or collect(38) = '1' or collect(39) = '1' or collect(40) = '1' or collect(49) = '1' or collect(50) = '1' or collect(51) = '1' or collect(52) = '1' ) then
		price <= "00110";

	elsif (collect(14) = '1' or collect(15) = '1' or collect(17) = '1' or collect(18) = '1' or collect(25) = '1' or collect(44) = '1' or collect(47) = '1' or collect(48) = '1' ) then 
		price <= "00010";

	elsif (collect(27) = '1' or collect(41) = '1' or collect(42) = '1' or collect(53) = '1' or collect(54) = '1' or collect(55) = '1') then
		price <= "01000";

	elsif (collect(56) = '1' or collect(57) = '1') then 
		price <= "01100";

	elsif (collect(58) = '1') then
		price <= "01110" ;

	else  price <= "00000";
	end if;
end process;

end archi;