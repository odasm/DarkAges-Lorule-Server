﻿///************************************************************************
//Project Lorule: A Dark Ages Server (http://darkages.creatorlink.net/index/)
//Copyright(C) 2018 TrippyInc Pty Ltd
//
//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.
//
//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
//GNU General Public License for more details.
//
//You should have received a copy of the GNU General Public License
//along with this program.If not, see<http://www.gnu.org/licenses/>.
//*************************************************************************/
namespace Darkages.Network.ClientFormats
{
    public class ClientFormat4A : NetworkFormat
    {
        public override bool Secured => true;

        public override byte Command => 0x4A;

        public uint Id { get; set; }
        public byte Type { get; set; }
        public byte ItemSlot { get; set; }
        public int Gold { get; set; }

        public override void Serialize(NetworkPacketReader reader)
        {
            Type = reader.ReadByte();
            Id = reader.ReadUInt32();

            if (Type == 0x01 && reader.CanRead)
                ItemSlot = reader.ReadByte();

            if (Type == 0x03 && reader.CanRead)
            {
                Gold = reader.ReadInt32();
            }
        }

        public override void Serialize(NetworkPacketWriter writer)
        {

        }
    }
}
